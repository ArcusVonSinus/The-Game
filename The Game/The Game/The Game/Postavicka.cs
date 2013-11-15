using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace The_Game
{
    class Postavicka
    {
        ///
        /// Konstanty
        /// 

        // ZDE JSOU NA VYBER RUZNE GRAVITACE
        const float gravitacniZrychleniNaZemi =  9.81373f;
        const float gravitacniZrychleniNaMesici = 1.62f;

        // ZDE SE MENI RYCHLOST POHYBU PANACKA
        const float standartniRychlost = 0.1f;
        const float standartniVyskok = 45f;
        const float padaciKonstanta = 0.005f;
        const float horizontalniZmenaPohybu = 1.73f;

        /// 
        /// Promenne
        /// 

        private AnimatedSprite[] vzhled; //"gif"    //0 doleva 1 doprava 2 vevzduchu ...
        private int vzhledNo;
        public Vector2 pozice, prevpozice; //souradnice    
        Vector2 pohyb; //smer pohybu
        public int width;//sirka v pixelech, prepocitana
        bool onLand;
        double mass;
        Speed rychlost;
        Background1 b;
        long elapsedTime = 0;


        /// 
        /// Konstruktor
        /// 

        public Postavicka(Texture2D[] textury, int typ, int X, int Y, int Width, double Mass, Speed speed, Background1 b)
        {
            if (typ == 0) //Me
            {
                vzhled    = new AnimatedSprite[4];
                vzhled[0] = new AnimatedSprite(textury[0], 2, 7);
                vzhled[1] = new AnimatedSprite(textury[1], 2, 7);
                vzhled[2] = new AnimatedSprite(textury[2], 2, 7);
                vzhled[3] = new AnimatedSprite(textury[3], 2, 7);
            }
            width = Width;
            pozice.X = X;
            pozice.Y = Y;
            prevpozice = pozice;
            mass = Mass;
            rychlost = speed;
            onLand = true;
            this.b = b;
        }


        /// 
        /// Metody
        /// 
        public void death()
        {
            throw new System.ArgumentException("You died", ":-("); 

        }
        public void update(GameTime gameTime)
        {
            prevpozice = pozice;

            /*v milisekundach*/
            long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime += timediff;
            float move;

            // Pri stisknuti leveho shiftu se zvysi rychlost na 1.5 nasobek standartni rychlosti
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                move = 1.5f * standartniRychlost * timediff;
            else
                move = standartniRychlost * timediff;
                        
            if (pozice.X <= 0) 
                pozice.X = 0;
            if (pozice.X >= b.sirka * 300 - 451) 
                pozice.X = b.sirka * 300 - 451; //jeste sirka panacka
            if ((pozice.X > (b.b - b.a) / 2) && (pozice.X < b.sirka * 300 - 301 - (b.b - b.a) / 2)) 
                b.move(pohyb.X);
            if (pozice.Y-261 > b.vyska*300)
            {
                death();
            }

            // PANACEK JE NA ZEMI
            if (onLand)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    pohyb.X = move;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    pohyb.X = -move;
                }
                else
                {
                    pohyb.X = 0f;
                }
                if (pohyb.X < 0) vzhledNo = 1;
                if (pohyb.X > 0) vzhledNo = 0;

                if (b.level1[(int)(pozice.X + 5) / 300, ((int)pozice.Y + 5) / 300].typ == 0)
                {
                    if (b.level1[(int)(pozice.X + 145) / 300, ((int)pozice.Y + 5) / 300].typ == 0)
                    {
                        onLand = false;
                        vzhledNo += 2;
                        vzhled[vzhledNo].Update();
                    }
                }
                pohyb.Y = 0f;

            }

            // ZMACKL JSEM MEZERNIK, ALE PANACEK JE JESTE NA ZEMI --> ZACINA SKOK
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onLand)
            {
                onLand = false;
                pozice.Y -= standartniVyskok * standartniRychlost;
                pohyb.Y = -3 * standartniVyskok * standartniRychlost / 5;
                if (pohyb.X < 0) vzhledNo = 2;
                if (pohyb.X > 0) vzhledNo = 3;
                if (pohyb.X == 0) vzhledNo += 2;
            }

            // PANACEK JE VE SKOKU / PADA (KAZDA SITUACE, KDY NESTOJI NA ZEMI)
            if (!onLand)
            {
                // nastaveni animace
                if (pohyb.X < 0) vzhledNo = 2;
                if (pohyb.X > 0) vzhledNo = 3;

                // anatomie skoku
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    pohyb.Y += gravitacniZrychleniNaZemi * padaciKonstanta;
                else
                {
                    if (pohyb.Y < 0) 
                        pohyb.Y = 0;
                    pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta;
                }

                // anatomie pohybu vpravo
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    pohyb.X += timediff * horizontalniZmenaPohybu * standartniRychlost;
                    vzhledNo = 2;
                }
                if (pohyb.X >= move) 
                    pohyb.X = move;

                // anatomie pohybu vlevo
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    pohyb.X -= timediff * horizontalniZmenaPohybu * standartniRychlost;
                    vzhledNo = 3;
                }
                if (pohyb.X <= -move) 
                    pohyb.X = -move;

                Vector2 temppozice = new Vector2();
                temppozice = pozice + pohyb;
                if ((b.level1[(int)(temppozice.X + 5) / 300, ((int)pozice.Y + 5) / 300].typ == 0) &&
                    (b.level1[(int)(temppozice.X + 145) / 300, ((int)temppozice.Y + 5) / 300].typ == 0))
                {

                }
                else
                {
                    onLand = true;
                    vzhledNo -= 2;
                    pohyb.Y = 0;
                    pohyb.X = 0;
                }
                


            }

            if (onLand && Math.Abs(pohyb.X) >= 0.1)
            {
                vzhled[vzhledNo].Update();
            }
            else if (onLand)
            {
                vzhled[vzhledNo].stop();
            }

            if (!onLand)
            {
                vzhled[vzhledNo].Update();
            }

            /*if (pozice.Y >= 1500)
            {
                if (!onLand && pohyb.X == 0)
                {
                    vzhledNo -= 2;
                }
                onLand = true;
                pozice.Y = 1500;
                if (pohyb.X < 0) vzhledNo = 1;
                if (pohyb.X > 0) vzhledNo = 0;
            }*/

            pozice += pohyb*timediff;
        }

        public int height
        {
            get
            {
                return (width * vzhled[0].Texture.Height) / vzhled[0].Texture.Width;
            }
            set { }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            temp = pozice;
            temp.X -= b.a;
            temp.Y -= 261;
            temp.Y += 13;
            temp.X *= (width / 150f);
            temp.Y *= (width / 150f);
            vzhled[vzhledNo].Draw(spriteBatch, temp, width);
        }
    }
}

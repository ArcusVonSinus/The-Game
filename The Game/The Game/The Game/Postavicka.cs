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
        const float standartniRychlost =1.6f;
        const float standartniVyskok = 1.2f;
        const float padaciKonstanta = 0.004f;
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
            /*v milisekundach*/
            long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime += timediff;
            float rychlostChuze;
            // Pri stisknuti leveho shiftu se zvysi rychlost na 1.5 nasobek standartni rychlosti
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                rychlostChuze = 1.5f * standartniRychlost;
            else
                rychlostChuze = standartniRychlost;
                                   

            // PANACEK JE NA ZEMI
            if (onLand)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    pohyb.X = rychlostChuze;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    pohyb.X = -rychlostChuze;
                }
                else
                {
                    pohyb.X = 0f;
                }

                if (pohyb.X < 0) vzhledNo = 1;
                if (pohyb.X > 0) vzhledNo = 0;

                if (b.level1[(int)(pozice.X + 5) / 300, ((int)pozice.Y + 20) / 300].typ == 0)
                {
                    if (b.level1[(int)(pozice.X + 145) / 300, ((int)pozice.Y + 20) / 300].typ == 0)
                    {
                        onLand = false;
                        vzhledNo += 2;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))            // ZMACKL JSEM MEZERNIK, ALE PANACEK JE JESTE NA ZEMI --> ZACINA SKOK
                {
                    onLand = false;                    
                    pohyb.Y = -3 * standartniVyskok * standartniRychlost;
                    if (pohyb.X < 0) vzhledNo = 2;
                    if (pohyb.X > 0) vzhledNo = 3;
                    if (pohyb.X == 0) vzhledNo += 2;
                }
            }
            else // PANACEK JE VE SKOKU / PADA (KAZDA SITUACE, KDY NESTOJI NA ZEMI)
            {
                // nastaveni animace
                if (pohyb.X < 0) vzhledNo = 2;
                if (pohyb.X > 0) vzhledNo = 3;

                // anatomie skoku
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta - standartniVyskok / 10;
                }
                else
                {
                    pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta;
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Right))   // anatomie pohybu vpravo
                {
                    pohyb.X += timediff * horizontalniZmenaPohybu * standartniRychlost;
                    vzhledNo = 2;
                }
                if (pohyb.X >= rychlostChuze)
                {
                    pohyb.X = rychlostChuze;
                }                
                if (Keyboard.GetState().IsKeyDown(Keys.Left))    // anatomie pohybu vlevo
                {
                    pohyb.X -= timediff * horizontalniZmenaPohybu * standartniRychlost;
                    vzhledNo = 3;
                }
                if (pohyb.X <= -rychlostChuze)
                {
                    pohyb.X = -rychlostChuze;
                }

                Vector2 temppozice = new Vector2();
                temppozice=pozice;
                for(int time = 1;time<=timediff;time++)
                {
                    temppozice+= pohyb;
                    if ((int)temppozice.Y + 5 < b.vyska * 300)
                    {
                        if ((b.level1[(int)(temppozice.X + 5) / 300, ((int)temppozice.Y + 5) / 300].typ == 0) &&
                            (b.level1[(int)(temppozice.X + 145) / 300, ((int)temppozice.Y + 5) / 300].typ == 0))
                        {

                        }
                        else
                        {
                            pozice += pohyb*(time-1);
                            onLand = true;
                            vzhledNo -= 2;
                            pohyb.Y = 0;
                            pohyb.X = 0;
                            break;
                        }
                    }
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

        

            /*zmena polohy*/
            pozice += pohyb*timediff;
            if (pozice.X <= 0)
                pozice.X = 0;
            if (pozice.X >= b.sirka * 300 - 451)
                pozice.X = b.sirka * 300 - 451; //jeste sirka panacka
            if ((pozice.X > (b.b - b.a) / 2) && (pozice.X < b.sirka * 300 - 301 - (b.b - b.a) / 2))
                /* b.move((int)pozice.X - ((b.b - b.a) / 2));
                 pol*/
                b.move(10);
            if (pozice.Y - 261 > b.vyska * 300)
            {
                death();
            }



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

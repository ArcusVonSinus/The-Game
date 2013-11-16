﻿using System;
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
        const float gravitacniZrychleniNaZemi = 9.81373f;
        const float gravitacniZrychleniNaMesici = 1.62f;

        // ZDE SE MENI RYCHLOST POHYBU PANACKA
        const float standartniRychlost = 1.6f;
        const float standartniVyskok = 2.5f;
        const float padaciKonstanta = 0.0035f;
        const float horizontalniZmenaPohybu = 0.5f;

        /// 
        /// Promenne
        /// 

        private AnimatedSpriteHead[] vzhled; //"gif"    //0 doleva 1 doprava 2 vevzduchu ...
        private int vzhledNo;
        public Vector2 pozice, prevpozice; //souradnice    
        Vector2 pohyb; //smer pohybu
        public int width;//sirka v pixelech, prepocitana (velikost vykresleneho obrazku)
        bool onLand;
        Speed rychlost;
        Background b;
        long elapsedTime = 0;
        bool skocil = false;
        bool nadKostkou;
        /// 
        /// Konstruktor
        /// 

        public Postavicka(Texture2D[][] textury, int X, int Y, int Width, Speed speed, Background b)
        {
            vzhled = new AnimatedSpriteHead[4];
            Vector3 pozicehlavy1 = new Vector3(0.7f, 0.11f, 0.06f);
            vzhled[0] = new AnimatedSpriteHead(textury[0], 2, 7, 6, 10, pozicehlavy1);
            vzhled[2] = new AnimatedSpriteHead(textury[2], 2, 7, 6, 10, pozicehlavy1);

            pozicehlavy1.Y = 0.15f;
            vzhled[1] = new AnimatedSpriteHead(textury[1], 2, 7, 6, 10, pozicehlavy1);
            vzhled[3] = new AnimatedSpriteHead(textury[3], 2, 7, 6, 10, pozicehlavy1);

            width = Width;
            pozice.X = X;
            pozice.Y = Y;
            prevpozice = pozice;
            rychlost = speed;
            onLand = true;
            this.b = b;
        }


        /// 
        /// Metody
        /// 
        public void death()
        {
            throw new System.ArgumentException("You died :-(", ":-(");

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
            if (!Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                skocil = false;

            }

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
                        if (vzhledNo <= 1)
                        {
                            vzhledNo += 2;
                        }
                    }
                }
                if (!skocil && Keyboard.GetState().IsKeyDown(Keys.Space))            // ZMACKL JSEM MEZERNIK, ALE PANACEK JE JESTE NA ZEMI --> ZACINA SKOK
                {
                    skocil = true;
                    onLand = false;
                    pozice.Y -= 5 * standartniVyskok * standartniRychlost / 3;
                    pohyb.Y = -standartniVyskok * standartniRychlost;
                    if (pohyb.X < 0) vzhledNo = 2;
                    if (pohyb.X > 0) vzhledNo = 3;
                    if (pohyb.X == 0 && vzhledNo <= 1) vzhledNo += 2;
                }
            }
            else // PANACEK JE VE SKOKU / PADA (KAZDA SITUACE, KDY NESTOJI NA ZEMI)
            {
                // nastaveni animace
                if (pohyb.X < 0) vzhledNo = 3;
                if (pohyb.X > 0) vzhledNo = 2;

                // anatomie skoku
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta - standartniVyskok / 8;
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
                temppozice = pozice;
                for (int time = 1; time <= timediff; time++)
                {
                    temppozice += pohyb;
                    if ((int)temppozice.Y + 5 < b.vyska * 300)
                    {
                        if ((b.level1[(int)(temppozice.X + 5) / 300, ((int)temppozice.Y - 5) / 300].typ == 0) &&
                            (b.level1[(int)(temppozice.X + 145) / 300, ((int)temppozice.Y - 5) / 300].typ == 0))
                        {
                            nadKostkou = true;
                        }
                        else if (pohyb.Y >= 0 && nadKostkou && temppozice.Y % 300 < 30)
                        {
                            pozice += pohyb * (time - 1);
                            onLand = true;
                            vzhledNo -= 2;
                            pohyb.Y = 0;
                            pohyb.X = 0;
                            break;
                        }
                        else
                        {
                            nadKostkou = false;
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
            vzhled[vzhledNo].UpdateHead();



            /*zmena polohy*/
            pozice += pohyb * timediff;
            if (pozice.X <= 0)
                pozice.X = 0;
            if (pozice.X >= b.sirka * 300 - 451)
                pozice.X = b.sirka * 300 - 451; //jeste sirka panacka
            if (((int)pozice.X > (b.b - b.a) / 4) && (pozice.X < b.sirka * 300 - 301 - 3 * (b.b - b.a) / 4))
            {
                b.move((int)pozice.X - b.a - ((b.b - b.a) / 4));
            }
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

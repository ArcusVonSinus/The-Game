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
    public class Postavicka
    {
        ///
        /// Konstanty
        /// 

        // ZDE JSOU NA VYBER RUZNE GRAVITACE
        protected const float gravitacniZrychleniNaZemi = 9.81373f;
        protected const float gravitacniZrychleniNaMesici = 1.62f;

        // ZDE SE MENI RYCHLOST POHYBU PANACKA
        protected const float standartniRychlost = 1.06f;
        protected const float standartniVyskok = 3f;
        protected const float padaciKonstanta = 0.001f;
        protected const float horizontalniZmenaPohybu = 0.12f;


        /// 
        /// Promenne
        /// 

        private AnimatedSpriteHead[] vzhled; //"gif"    //0 doleva 1 doprava 2 vevzduchu doleva 3 vevzduchu doprava ...
        protected int vzhledNo; //ktery vzhled je prave zobrazen
        public Vector2 pozice, prevpozice; //souradnice    
        public Vector2 pohyb; //smer pohybu
        public int width,height;//sirka v pixelech, prepocitana (velikost vykresleneho obrazku)
        protected bool onLand;
        protected Background b;
        protected long elapsedTime = 0;
        bool skocil = false;   //drzim-li mezernik, tak mam skocit jen jednou, ne skakat porad
        protected Game1 game;
        public virtual Rectangle kolizniObdelnik
        {
            get
            {
                return new Rectangle((int)pozice.X + 15, (int)pozice.Y - 261 + 26, 150 - 2 * 15, 261 - 2 * 26);
            }
            set { }
        }
        public bool odrazOdPriserky = false;

        /// 
        /// Konstruktor
        /// 

        public Postavicka()
        {

        }
        public Postavicka(Game1 game, Texture2D[][] textury, int X, int Y, Background b)
        {
            this.game = game;
            vzhled = new AnimatedSpriteHead[4];
            Vector3 poziceHlavy1 = new Vector3(0.7f, 0.11f, 0.06f);
            vzhled[0] = new AnimatedSpriteHead(textury[0], 2, 7, 6, 10, poziceHlavy1);
            vzhled[2] = new AnimatedSpriteHead(textury[2], 2, 7, 6, 10, poziceHlavy1);

            poziceHlavy1.Y = 0.15f;
            vzhled[1] = new AnimatedSpriteHead(textury[1], 2, 7, 6, 10, poziceHlavy1);
            vzhled[3] = new AnimatedSpriteHead(textury[3], 2, 7, 6, 10, poziceHlavy1);
            pozice.X = X;
            pozice.Y = Y;
            prevpozice = pozice;
            onLand = true;            
            this.b = b;
        }

        /// 
        /// Metody
        /// 
        public virtual void death()
        {
            game.InGame = false; 
            game.InMenu = true;
            game.IsMouseVisible = true;
            game.m.ktereMenu = KtereMenu.main; 

        }
        public virtual void update(GameTime gameTime)
        {
            width = game.blockSize / 2;  //v pripade zmeny rozliseni se musi prepocitat sirka
            height = (width * this.vzhled[0].Texture.Height) / vzhled[0].Texture.Width;
            long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime += timediff; //v millisekundach
            if (timediff > 200)   //jsem-li v menu, tak stale bezi cas a timediff je pak nekolik tisic (nekolik sekund), takze bych se pohl o strasne velky kus. Tohle to vyresi. Pokud by fps bylo < 5, tak je to stejne nehratelne.
                timediff = 16;    
            float rychlostChuze;
            // Pri stisknuti leveho shiftu se zvysi rychlost na 1.5 nasobek standartni rychlosti
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                rychlostChuze = 1.5f * standartniRychlost;
            else
                rychlostChuze = standartniRychlost;

            if (!(Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)))
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

                if (b.level[(int)(pozice.X + 5) / 300, ((int)pozice.Y + 20) / 300].typ == 0)
                {
                    if (b.level[(int)(pozice.X + 145) / 300, ((int)pozice.Y + 20) / 300].typ == 0)
                    {
                        onLand = false; //nemam-li nohy na zemi, zacinam litat :-)
                        if (vzhledNo <= 1)    
                        {
                            vzhledNo += 2;
                        }
                    }
                }
                if (!skocil && (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)))
                // ZMACKL JSEM MEZERNIK, ALE PANACEK JE JESTE NA ZEMI --> ZACINA SKOK
                {
                    skocil = true;
                    onLand = false;
                    pozice.Y -= 5 * standartniVyskok / 3;
                    pohyb.Y = -standartniVyskok;
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
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up)))
                {
                      pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta;
                }
                else
                {
                    if (!odrazOdPriserky)
                    {
                        if (pohyb.Y < 0)
                            pohyb.Y = 0;
                        pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta;
                    }
                    else
                        pohyb.Y += timediff * gravitacniZrychleniNaZemi * padaciKonstanta;
                }
                if (pohyb.Y > 0)
                    odrazOdPriserky = false;
                if (pohyb.Y < -4f)
                    pohyb.Y = -4f;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                // anatomie pohybu vpravo
                {
                    pohyb.X += timediff * horizontalniZmenaPohybu * standartniRychlost;
                    vzhledNo = 2;
                    if (pohyb.X >= rychlostChuze)
                    {
                        pohyb.X = rychlostChuze;
                    }
                }
                else
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    // anatomie pohybu vlevo
                    {
                        pohyb.X -= timediff * horizontalniZmenaPohybu * standartniRychlost;
                        vzhledNo = 3;
                        if (pohyb.X <= -rychlostChuze)
                        {
                            pohyb.X = -rychlostChuze;
                        }
                    }
                    else pohyb.X *= 0.75f;
                                
                Vector2 temppozice = new Vector2();
                temppozice = pozice;
                for (int time = 1; time <= timediff; time++)   //mam se pohnout o timediff*pohyb, a timediff je int, takze ho projedu a vzdy zkusim, zdali uz nahodou nekoliduu s prostredim
                {
                    temppozice += pohyb;
                    if ((int)temppozice.Y + 5 < b.vyska * 300)
                    {
                        if ((b.level[(int)(temppozice.X + 5) / 300, ((int)temppozice.Y - 5) / 300].typ == 0) &&
                            (b.level[(int)(temppozice.X + 145) / 300, ((int)temppozice.Y - 5) / 300].typ == 0))
                        {

                        }
                        else if (pohyb.Y >= 0 && (int)(temppozice.Y - 5) % 300 < 30)
                        {
                            pozice += pohyb * (time - 1);
                            onLand = true;
                            vzhledNo -= 2;
                            pohyb.Y = 0;
                            pohyb.X = 0;
                            break;
                        }
                    }
                }
            }

            if (onLand && Math.Abs(pohyb.X) > 0)
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
                pozice.X = 0; // neprejde za levy okraj
            if (pozice.X >= b.sirka * 300 - 451)
                pozice.X = b.sirka * 300 - 451; // neprejde za pravy okraj
            if (((int)pozice.X > (b.b - b.a) / 4) && (pozice.X < b.sirka * 300 - 301 - 3 * (b.b - b.a) / 4))
            {
                b.move((int)pozice.X - b.a - ((b.b - b.a) / 4));
            }
            if ((pozice.Y - 261 > b.vyska * 300) || (pozice.Y < 0))
            {
                death();
            }
        }
        public void gain(int kolik)
        {

        }       
        public virtual void draw(SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            temp = pozice;
            temp.X -= b.a; //prizpusobeni se pozadi
            temp.Y -= 261; //prepocet z leveho dolniho rohu na pravy dolni
            temp.Y += 13; //aby to lepe sedelo na pozadi
            temp.X *= (width / 150f); // prepocet na skutecne pixely
            temp.Y *= (width / 150f);
            vzhled[vzhledNo].Draw(spriteBatch, temp, width);
        }
    }
}

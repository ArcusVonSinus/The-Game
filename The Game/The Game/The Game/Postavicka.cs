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
        //Konstany
        const float gravitacniZrychleniNaZemi = 9.81f;
        const float gravitacniZrychleniNaMesici = 1.62f;
        
        // Promenne
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
        // Konstruktor
        public Postavicka(Texture2D[] textury,int typ, int X, int Y, int Width, double Mass, Speed speed,Background1 b)
        {
            if (typ == 0) //Me
            {
                vzhled = new AnimatedSprite[4];
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

        // Metody
        public void update(GameTime gameTime)
        {

            prevpozice = pozice;

            /*v ms*/ long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime+=timediff;
            int step = 1;
            float move;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                move = 1.15f*timediff;
            else
                move = 0.77f*timediff;
            


            b.move(1+0*timediff);

            pozice += pohyb;
            
            
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

                
                pohyb.Y = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onLand)
            {
                pozice.Y -= 25f*step;
                pohyb.Y = -15f*step;
                onLand = false;
                if (pohyb.X < 0) vzhledNo = 2;
                if (pohyb.X > 0) vzhledNo = 3;
                if (pohyb.X == 0) vzhledNo += 2;
            }


            if (!onLand)
            {
                if (pohyb.X < 0) vzhledNo = 2;
                if (pohyb.X > 0) vzhledNo = 3;
                float i = gravitacniZrychleniNaZemi;
                pohyb.Y += 0.08f * i * step;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    pohyb.X += 0.3f*step;
                    vzhledNo = 2;
                }
                if (pohyb.X >= move) pohyb.X = move;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    pohyb.X -= 0.3f*step;
                    vzhledNo = 3;
                }
                if (pohyb.X <= -move) pohyb.X = -move; 
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

            if (pozice.Y >= 550)
            {
                if (!onLand && pohyb.X == 0)
                {
                    vzhledNo -= 2;
                }
                onLand = true;
                if (pohyb.X < 0) vzhledNo = 1;
                if (pohyb.X > 0) vzhledNo = 0;

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

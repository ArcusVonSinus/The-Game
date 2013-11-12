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
        public Vector2 pozice; //souradnice    
        Vector2 pohyb; //smer pohybu
        public int width;//sirka v pixelech
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
            mass = Mass;
            
            rychlost = speed;
            onLand = true;
            this.b = b;
        }

        // Metody
        public void update(GameTime gameTime)
        {
            /*v ms*/ long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime+=timediff;
            int step=(int) timediff/13;         
            b.move(5*step);

            pozice += pohyb;
            float move;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                move = 15f*step;
            else
                move = 10f*step;
            
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

        public int height()
        {
            int h;
            h = (width * vzhled[0].Texture.Height) / vzhled[0].Texture.Width;
            return h;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            vzhled[vzhledNo].Draw(spriteBatch, pozice, width);
        }

    }
}

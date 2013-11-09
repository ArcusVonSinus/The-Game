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
                
        // Konstruktor
        public Postavicka(Texture2D[] textury,int typ, int X, int Y, int Width, double Mass, Speed speed)
        {
            if (typ == 0) //Me
            {
                vzhled = new AnimatedSprite[2];
                vzhled[0] = new AnimatedSprite(textury[0], 2, 7);
                vzhled[1] = new AnimatedSprite(textury[1], 2, 7);
            }
            width = Width;
            pozice.X = X;
            pozice.Y = Y - height()-120;
            mass = Mass;
            
            rychlost = speed;
            onLand = true;
        }

        // Metody
        public void update(GameTime gameTime)
        {
            pozice += pohyb;

            if (onLand)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) pohyb.X = 10f;
                else if (Keyboard.GetState().IsKeyDown(Keys.Left)) pohyb.X = -10f;
                else pohyb.X = 0f;

                if (pohyb.X < 0) vzhledNo = 1;
                if (pohyb.X > 0) vzhledNo = 0;

                pohyb.Y = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && onLand)
            {
                pozice.Y -= 25f;
                pohyb.Y = -15f;
                onLand = false;
            }


            if (!onLand)
            {                
                float i = gravitacniZrychleniNaZemi;
                pohyb.Y += 0.08f * i;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    pohyb.X += 0.3f;
                    vzhledNo = 0;
                }
                if (pohyb.X >= 10f) pohyb.X = 10f;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    pohyb.X -= 0.3f;
                    vzhledNo = 1;
                }
                if (pohyb.X <= -10f) pohyb.X = -10f; 
            }
            
            if (Math.Abs(pohyb.X) >= 0.1) vzhled[vzhledNo].Update();
            else vzhled[vzhledNo].stop();

            if (pozice.Y >= 520)
                onLand = true;                        
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

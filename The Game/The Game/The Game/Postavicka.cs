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
        private AnimatedSprite[] vzhled; //"gif"    //0 doleva 1 doprava 2 vevzduchu ...
        private int vzhledNo;
        private int x, y; //souradnice
        private double mass;  //hmotnost
        int width;//sirka v pixelech
        bool onLand;
        public Speed rychlost;

        public Postavicka(Texture2D[] textury,int typ, int X, int Y, int Width, double Mass, Speed speed)
        {
            if (typ == 0) //Me
            {
                vzhled = new AnimatedSprite[2];
                vzhled[0] = new AnimatedSprite(textury[0], 2, 7);
                vzhled[1] = new AnimatedSprite(textury[1], 2, 7);
            }
            x = X;
            y = Y;
            mass = Mass;
            width = Width;
            rychlost = speed;
            onLand = true;
        }
        public void update()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {                
                rychlost.x = 10;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rychlost.x = -10;
            }
            else
            {
                rychlost.x = 0;
            }
            x = Convert.ToInt32(x + rychlost.x);
            y = Convert.ToInt32(y + rychlost.y);


            if (!onLand)
            {
                vzhledNo = 2;
            }
            else if (rychlost.x < 0)
            {
                vzhledNo = 1;
            }
            else if (rychlost.x >= 0)
            {
                vzhledNo = 0;
            }
            if (Math.Abs(rychlost.x) >= 0.1)
            {
                vzhled[vzhledNo].Update();
            }
            else
            {
                vzhledNo = 0;
                vzhled[0].stop();
            }



        }
        public void draw(SpriteBatch spriteBatch)
        {
            vzhled[vzhledNo].Draw(spriteBatch, new Vector2(x, y), width);
        }

    }
}

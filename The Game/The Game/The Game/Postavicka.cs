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
        private AnimatedSprite vzhled; //"gif"
        private int x, y; //souradnice
        private double mass;  //hmotnost
        int width; //sirka v pixelech

        public Postavicka(AnimatedSprite design,int X, int Y, double Mass,int Width)
        {
            vzhled = design;
            x = X;
            y = Y;
            mass = Mass;
            width = Width;
        }
        public void update()
        {
            vzhled.Update();
        }
        public void draw(SpriteBatch spriteBatch)
        {
            vzhled.Draw(spriteBatch, new Vector2(x, y), width);
        }
    }
}

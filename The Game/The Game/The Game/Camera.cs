using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Game
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        public Vector2 centre;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gametime, Postavicka me)
        {
            centre = new Vector2(me.pozice.X + me.width/2 -700, 0);
            transform = Matrix.CreateScale(new Vector3(1,1,0)) * 
                Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y,0));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game
{

    class Background1
    {
        int a; //je zobrazeno pozadí od a do b;
        int b;
        int [,] level1;
        
        public Background1()
        {
            string[] level;
            level = new string[7];
            level[0] = "..............................";
            level[1] = "..............................";
            level[2] = "..............................";
            level[3] = "..............................";
            level[4] = "..............................";
            level[5] = "..............................";
            level[6] = "..............................";
            level[7] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
           // for(int x = 0;x<=
        }
       /* public int barva(int h)
        {
           // return (image1.GetPixel(0, h)).R;
        }
        */
    }
}

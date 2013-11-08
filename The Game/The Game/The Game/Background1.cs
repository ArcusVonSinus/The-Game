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
        
        public Background1(int radku)
        {
            System.IO.StreamReader lvlreader = new System.IO.StreamReader(@"Content/l1.txt");
            string levelline=lvlreader.ReadLine();
            int sirka=levelline.Length;
            level1=new int [sirka,radku];
            for (int j=0;j<radku;j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    level1[i, j] = levelline[i];

                }
                if(j!=radku-1)
                    levelline = lvlreader.ReadLine();

            }
            //int u = 7; ???na co to tu je???
        }

    }
}

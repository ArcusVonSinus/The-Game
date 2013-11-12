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
        /// <summary>
        /// Tile je z definice 300x300, a souradnice a a b jsou v tomhle meritku, pak se prepocitaji
        /// </summary>
        int a; //je zobrazeno pozadí od a do b;
        int b;
        int [,] level1;
        int sirka; //pocet dlazdic na sirce levelu;
        int vyska;
        Texture2D[][] pozadi;
        public Background1(Texture2D[][] pozadi,int radku)
        {
            a = 0; b = 4200;
            this.pozadi = pozadi;
            System.IO.StreamReader lvlreader = new System.IO.StreamReader(@"Content/l1.txt");
            string levelline=lvlreader.ReadLine();
            sirka=levelline.Length;
            vyska = radku;
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

            for (int j = 0; j < vyska; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    if (level1[i, j] == 'X') 
                        level1[i, j] = 1;
                    if (level1[i, j] == 'x') 
                        level1[i, j] = 2;
                    if (level1[i, j] == '.')
                        level1[i, j] = 0;
                }
            }

        }
        public void draw(int w,int h,SpriteBatch spriteBatch)
        {
            int a1 = a/300;
            int b1 = b/300;
            b1++;
            int posunL = 300-(a%300);
            int sirkabunky = h/vyska;
            for (int r = 0; r < vyska; r++)
            {
                for (int s = 0; s < b1 - a1+2; s++)
                {
                    spriteBatch.Draw(pozadi[level1[a1 + s,r]][0], new Rectangle(sirkabunky * s - posunL, sirkabunky*r, sirkabunky, sirkabunky), Color.White);
                }
            }
        }


    }
}

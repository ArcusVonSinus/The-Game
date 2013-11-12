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
        struct Tile
        {
            public int typ;
            public int verze;
        }
        /// <summary>
        /// Tile je z definice 300x300, a souradnice a a b jsou v tomhle meritku, pak se prepocitaji
        /// </summary>
        int a; //je zobrazeno pozadí od a do b;
        int b;
        Tile [,] level1;
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
            level1=new Tile [sirka,radku];
            Random rnd = new Random();
            for (int j=0;j<radku;j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    level1[i, j].typ = levelline[i];
                    

                }
                if(j!=radku-1)
                    levelline = lvlreader.ReadLine();

            }

            for (int j = 0; j < vyska; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    if (level1[i, j].typ == 'X') 
                    {
                        level1[i, j].typ = 1;
                        level1[i,j].verze = rnd.Next(0,pozadi[1].Length);
                    }
                    if (level1[i, j].typ == 'x') 
                    {
                        level1[i, j].typ = 2;
                        level1[i,j].verze = rnd.Next(0,pozadi[2].Length);
                    }
                    if (level1[i, j].typ == '.')
                    {
                        level1[i, j].typ = 0;
                        level1[i,j].verze = rnd.Next(0,pozadi[0].Length);
                    }
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
                    spriteBatch.Draw(pozadi[level1[a1 + s, r].typ][level1[a1 + s, r].verze], new Rectangle(sirkabunky * s - posunL, sirkabunky * r, sirkabunky, sirkabunky), Color.White);
                }
            }
        }


    }
}

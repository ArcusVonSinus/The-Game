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
        public void move(int okolik)
        {
            int tempb = b;
            int tempa = a;
            b += okolik;
            a += okolik;
            if (b >= sirka * 300-300)
            {
                b = sirka * 300-301;
                a = tempa + b - tempb;
                return;
            }
        }
        public Background1(Texture2D[][] pozadi,int radku,int b)
        {
            a = 0; 
            this.b = b;
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
            lvlreader.Close();
            string Dily;
            Dily = ".XxLlvRrAaBbCc";
            int verzi = 0;
            for (int j = 0; j < vyska; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    for (int k = 0; k < Dily.Length; k++)
                    {
                        if (level1[i, j].typ == Dily[k])
                        {
                            level1[i, j].typ = k;
                            if (k == 0)
                                verzi = 3;
                            else if (k == 1)
                                verzi = 2;
                            else
                                verzi = 1;
                            level1[i, j].verze = rnd.Next(0, verzi);
                        }

                    }
                }
            }

        }
        public void draw(int w,int h,SpriteBatch spriteBatch)
        {
            int a1 = a/300;
            int b1 = b/300;
            b1++;
            int posunL = a%300;
            int sirkabunky = h / vyska;
            posunL *= sirkabunky;
            posunL /= 300;            
            for (int r = 0; r < vyska; r++)
            {
                for (int s = a1; s <= b1; s++)
                {
                    spriteBatch.Draw(pozadi[level1[s, r].typ][level1[s, r].verze], new Rectangle((s-a1)*sirkabunky - posunL, sirkabunky * r, sirkabunky, sirkabunky), Color.White);
                }
            }
        }
        
    }
}

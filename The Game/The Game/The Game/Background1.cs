using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game
{
    public class Score
    {
        public int score;
        Game1 game;
        Texture2D pozadi;
        Text sc;
        Vector2 size;
        Vector2 position;
        public Score(Game1 game)
        {
            this.game = game;
            pozadi = game.Content.Load<Texture2D>("Level " + game.level + "/scoreBackground");
            score = 0;
            size = new Vector2();
            position = new Vector2();
            sc = new Text(game, (int)(game.width - size.X - 0.5f * size.Y), (int)(0.5f * size.Y), (int)(0.075f * game.height),"0", "Score" + game.level,true);
        }
        public void draw(SpriteBatch spriteBatch)
        {
            size.Y = 0.075f * game.height;
            size.X = 5 * size.Y;
            position.X = game.width - size.X - 0.5f * size.Y;
            position.Y = 0.5f * size.Y;
            spriteBatch.Draw(pozadi, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
            Vector2 textpos = new Vector2(position.X,position.Y);

            textpos.X+=size.X - game.m.zmenseni * 20;
            textpos.Y += game.m.zmenseni * 7;
            sc.ChangeLoc(textpos, (int)(size.Y));
            sc.ChangeText(score.ToString());
            sc.Draw(spriteBatch);            
        }
    }
    public class Background
    {
        public struct Tile
        {
            public int typ;
            public int verze;
        }
        /// <summary>
        /// Tile je z definice 300x300, a souradnice a a b jsou v tomhle meritku, pak se prepocitaji
        /// </summary>
        public int a, b; //je zobrazeno pozadí od a do b (horizontalne)
        public Tile[,] level;
        Tile[,] obloha1;
        public int sirka; //pocet dlazdic na sirce levelu;
        public int vyska;
        Texture2D[][] pozadi;
        Game1 game;
        public Score score;

        public Background(Game1 game, Texture2D[][] pozadi, int radku, int b)
        {
            this.game = game;
            score = new Score(game);
            a = 0;
            this.b = b;
            this.pozadi = pozadi;
            System.IO.StreamReader lvlReader = new System.IO.StreamReader(@"Content/Level " + game.level + "/l" + game.level + ".txt");
            string levelLine = lvlReader.ReadLine();
            sirka = levelLine.Length;
            vyska = radku;
            level = new Tile[sirka, radku];
            obloha1 = new Tile[sirka, radku];
            Random rnd = new Random();
            for (int j = 0; j < radku; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    level[i, j].typ = levelLine[i];
                }
                if (j != radku - 1)
                    levelLine = lvlReader.ReadLine();
            }
            lvlReader.Close();
            string Dily = ".?X?R?L?l?x?r?";
            int verzi = 0;
            for (int j = 0; j < vyska; j++)
            {
                for (int i = 0; i < sirka; i++)
                {
                    for (int k = 0; k < Dily.Length; k++)
                    {

                        level[i, j].verze = rnd.Next(0, verzi);
                        if (j == vyska - 1)
                        {
                            obloha1[i, j].typ = 1;
                            obloha1[i, j].verze = rnd.Next(0, 1);
                        }
                        else
                        {
                            obloha1[i, j].typ = 0;
                            obloha1[i, j].verze = rnd.Next(0, 3);
                        }

                        if (level[i, j].typ == Dily[k])
                        {
                            level[i, j].typ = k;
                            switch (game.level)
                            {
                                case 1:
                                    if (k == 0)
                                        verzi = 3;
                                    else if (k == 2)
                                        verzi = 2;
                                    else
                                        verzi = 1;
                                    break;
                                case 2:
                                    if (k == 0)
                                        verzi = 1;
                                    else if (k == 2)
                                        verzi = 2;
                                    else
                                        verzi = 1;
                                    break;

                            }
                        }
                    }
                    if (level[i, j].typ >= '0' && level[i, j].typ <= '9')
                    {
                        game.zoo.add(level[i, j].typ - '0', i * 300, j * 300, this);
                        level[i, j].typ = 0;
                    }
                    if (level[i, j].typ == '*')
                    {
                        level[i, j].typ = 0;
                    }
                }
            }
        }

        public void move(float okolik)
        { move((int)okolik); return; }
        public void move(int okolik)
        {
            int tempB = b;
            int tempA = a;
            b += okolik;
            a += okolik;
            if (b >= sirka * 300 - 300)
            {
                b = sirka * 300 - 301;
                a = tempA + b - tempB;
                return;
            }
            if (a <= 1)
            {
                a = tempA;
                b = tempB;
            }
        }
        public void draw(int w, int h, SpriteBatch spriteBatch)
        {
            int a1 = a / 300;
            int b1 = b / 300;
            b1++;
            int posunL = a % 300;
            int sirkabunky = h / vyska;
            posunL *= sirkabunky;
            posunL /= 300;
            for (int r = 0; r < vyska; r++)
            {
                for (int s = a1; s <= b1; s++)
                {
                    spriteBatch.Draw(pozadi[obloha1[s, r].typ][obloha1[s, r].verze], new Rectangle((s - a1) * sirkabunky - posunL, sirkabunky * r, sirkabunky, sirkabunky), Color.White);
                    if (level[s, r].typ != 0)
                    {
                        spriteBatch.Draw(pozadi[level[s, r].typ][level[s, r].verze], new Rectangle((s - a1) * sirkabunky - posunL, sirkabunky * r, sirkabunky, sirkabunky), Color.White);
                    }
                    if (level[s, r].typ <= 12 && level[s, r].typ >= 2 && level[s, r].typ % 2 == 0 && r != 0)
                    {
                        spriteBatch.Draw(pozadi[level[s, r].typ + 1][0], new Rectangle((s - a1) * sirkabunky - posunL, sirkabunky * (r - 1), sirkabunky, sirkabunky), Color.White);
                    }
                }
            }
            score.draw(spriteBatch);
        }
    }
}

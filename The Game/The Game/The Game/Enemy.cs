using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game
{
    class Enemy
    {
        Game1 game;
        int vzhledNo = 0;
        Vector2 poloha;
        AnimatedSprite[] vzhled;
        int typ;
        public Enemy(Game1 game, int X, int Y, int typ) 
        {
            this.typ = typ;
            poloha = new Vector2(X,Y);
            this.game = game;
            int pohybu=1;
            int radky=1,sloupcu=1;
            switch(typ)
            {
                case 0:
                pohybu = 2;
                radky = 4;
                sloupcu = 4;
                break;
                case 1:
                pohybu = 1;
                radky = 2;
                sloupcu = 3;
                break;
                case 3:
                pohybu = 1;
                radky = 1;
                sloupcu = 2;
                break;

            }
            
            vzhled = new AnimatedSprite[pohybu];

            vzhled[0] = new AnimatedSprite(game.Content.Load<Texture2D>("Level " + game.level + "/Enemy/E" + typ), radky, sloupcu);
            if(pohybu>=2)
                vzhled[1] = new AnimatedSprite(game.Content.Load<Texture2D>("Level " + game.level + "/Enemy/E" + typ + "1"), radky, sloupcu);

            
        }
        public void update()
        {
            vzhled[vzhledNo].Update();
            if (typ == 0)
            {
                if (game.me.pozice.X + 75 < poloha.X + 150)
                {
                    vzhledNo = 1;
                }
                else if (game.me.pozice.X + 75 > poloha.X + 150)
                {
                    vzhledNo = 0;
                }
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            temp= poloha;
            temp.X -= game.b.a;
            temp.X *= (game.blockSize / 300f);
            temp.Y *= (game.blockSize / 300f);
            vzhled[vzhledNo].Draw(spriteBatch, temp, game.blockSize);
        }
    }
    public class Zoo
    {
        Game1 game;
        List<Enemy> zoo;
        public Zoo(Game1 game)
        {
            this.game = game;
            zoo = new List<Enemy>();
        }
        public void add(int typ, int x, int y)
        {
            Enemy temp = new Enemy(game, x, y, typ);
            zoo.Add(temp);
        }

        public void Update()
        {
            foreach (Enemy e in zoo)
            {
                e.update();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy e in zoo)
            {
                e.draw(spriteBatch);
            }
        }
    }


}

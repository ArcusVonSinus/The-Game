using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game
{
    class Enemy : Postavicka
    {
        AnimatedSprite[] vzhled;
        int typ;
        public Enemy(Game1 game, int X, int Y, int typ)
        {
            this.typ = typ;
            pozice = new Vector2(X, Y);
            this.game = game;
            int pohybu = 1;
            int radky = 1, sloupcu = 1;
            switch (typ)
            {
                case 0:
                    pohyb.X = 0;
                    pohyb.Y = 0;
                    pohybu = 2;
                    radky = 4;
                    sloupcu = 4;
                    break;
                case 1:
                    pohyb.X = 0;
                    pohyb.Y = 0;
                    pohybu = 1;
                    radky = 2;
                    sloupcu = 3;
                    break;
                case 3:
                    pohyb.X = 0;
                    pohyb.Y = 0;
                    pohybu = 1;
                    radky = 1;
                    sloupcu = 2;
                    break;
            }
            Random rnd = new Random();
            int tatoNahodaUrciPocatecniSmer = rnd.Next(2);
            for (int i = 0; i <= tatoNahodaUrciPocatecniSmer; i++)
                this.pohyb.X *= -1;
            vzhled = new AnimatedSprite[pohybu];

            vzhled[0] = new AnimatedSprite(game.Content.Load<Texture2D>("Level " + game.level + "/Enemy/E" + typ), radky, sloupcu);
            if (pohybu >= 2)
                vzhled[1] = new AnimatedSprite(game.Content.Load<Texture2D>("Level " + game.level + "/Enemy/E" + typ + "1"), radky, sloupcu);

        }
        public override void update(GameTime gameTime)
        {
            width = game.blockSize / 2;
            /*v milisekundach*/
            long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime += timediff;
            float rychlostPriserky = standartniRychlost;
            
            vzhled[vzhledNo].Update();

            if (typ == 0)
            {
                this.pohyb.X = rychlostPriserky;
                this.pozice += this.pohyb;
                if (game.me.pozice.X + 75 < this.pozice.X + 150)
                {
                    vzhledNo = 1;
                }
                else if (game.me.pozice.X + 75 > this.pozice.X + 150)
                {
                    vzhledNo = 0;
                }
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            temp = pozice;
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

        public void Update(GameTime gameTime)
        {
            foreach (Enemy e in zoo)
            {
                e.update(gameTime);
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

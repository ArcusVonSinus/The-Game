using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace The_Game
{
    public class Enemy : Postavicka
    {
        AnimatedSprite[] vzhled;
        int typ;
        Vector2 zakladniPozice;
        public Enemy(Game1 game, int X, int Y, int typ, Background b)
        {
            this.typ = typ;
            pozice = new Vector2(X, Y);
            zakladniPozice = new Vector2(X, Y);
            this.game = game;
            int pohybu = 1;
            int radky = 1, sloupcu = 1;
            this.b = b;
            switch (typ)
            {
                case 0:
                    pohyb.X = standartniRychlost * 0.937465f;
                    pohyb.Y = 0;
                    pohybu = 2;
                    radky = 4;
                    sloupcu = 4;
                    break;
                case 1:
                    pohyb.X = standartniRychlost * 0.7f;
                    pohyb.Y = standartniRychlost * 0.3f;
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
            kolizniObdelnik = new Rectangle((int)(X + this.width / 10.00), (int)(Y + this.height / 10.00), (int)(this.width * 0.8), (int)(this.height * 0.8));
        }
        public override void update(GameTime gameTime)
        {
            /*v milisekundach*/
            long timediff = gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 60 * 1000 + gameTime.TotalGameTime.Hours * 24 * 60 * 1000 - elapsedTime;
            elapsedTime += timediff;

            vzhled[vzhledNo].Update();

            switch (typ)
            {
                case 0: // TRISECTOR
                    if (this.pohyb.X < 0)
                    {
                        vzhledNo = 1;
                    }
                    else if (this.pohyb.X > 0)
                    {
                        vzhledNo = 0;
                    }

                    /*zmena polohy*/
                    this.pozice += this.pohyb * timediff;
                    if (this.pozice.X < 0)
                    {
                        if (this.pohyb.X < 0)
                        {
                            this.pozice.X = 0; // neprejde za levy okraj
                            this.pohyb.X *= -1; // a otoci se
                        }
                    }
                    if (this.pozice.X > b.sirka * 300 - 600)
                    {
                        if (this.pohyb.X > 0)
                        {
                            this.pozice.X = b.sirka * 300 - 600; // neprejde za pravy okraj
                            this.pohyb.X *= -1; // a otoci se
                        }
                    }
                    if ((this.pohyb.X < 0) &&
                        ((int)(this.pozice.X) / 300 >= 0))
                        if ((b.level[(int)(this.pozice.X + 250) / 300, ((int)this.pozice.Y + 420) / 300].typ == 6) ||
                            (b.level[(int)(this.pozice.X + 250) / 300, ((int)this.pozice.Y + 420) / 300].typ == 8))
                            this.pohyb.X *= -1;
                    if (this.pohyb.X > 0)
                        if ((b.level[(int)(this.pozice.X + 25) / 300, ((int)this.pozice.Y + 420) / 300].typ == 4) ||
                            (b.level[(int)(this.pozice.X + 25) / 300, ((int)this.pozice.Y + 420) / 300].typ == 12))
                            this.pohyb.X *= -1;
                    break;

                case 1: // IRACIONALNI ZRUDA
                    /*zmena polohy*/
                    this.pozice += this.pohyb * timediff;
                    if (this.pozice.X + 300 < this.zakladniPozice.X)
                    {
                        if (this.pohyb.X < 0)
                        {
                            this.pozice.X = this.zakladniPozice.X - 300; // neodejde od sve zakldani pozice dal nez na zadanou vzdalenost (LEVA STRANA)
                            this.pohyb.X *= -1; // a otoci se
                        }
                    }
                    if (this.pozice.X - 300 > this.zakladniPozice.X)
                    {
                        if (this.pohyb.X > 0)
                        {
                            this.pozice.X = this.zakladniPozice.X + 300; // neodejde od sve zakldani pozice dal nez na zadanou vzdalenost (PRAVA STRANA)
                            this.pohyb.X *= -1; // a otoci se
                        }
                    }
                    if (this.pozice.Y + 45 < this.zakladniPozice.Y)
                    {
                        if (this.pohyb.Y < 0)
                        {
                            this.pozice.Y = this.zakladniPozice.Y - 45; // neodejde od sve zakldani pozice dal nez na zadanou vzdalenost (SMER DOLU)
                            this.pohyb.Y *= -1; // a otoci se
                        }
                    }
                    if (this.pozice.Y - 45 > this.zakladniPozice.Y)
                    {
                        if (this.pohyb.Y > 0)
                        {
                            this.pozice.Y = this.zakladniPozice.Y + 45; // neodejde od sve zakldani pozice dal nez na zadanou vzdalenost (SMER NAHORU)
                            this.pohyb.Y *= -1; // a otoci se
                        }
                    }
                    break;
            }
            this.kolizniObdelnik.X += (int)(this.pohyb.X * timediff);
            this.kolizniObdelnik.Y += (int)(this.pohyb.Y * timediff);
            
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
        public List<Enemy> zoo;
        public Zoo(Game1 game)
        {
            this.game = game;
            zoo = new List<Enemy>();
        }
        public void add(int typ, int x, int y, Background b)
        {
            Enemy temp = new Enemy(game, x, y, typ, b);
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

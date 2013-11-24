using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace The_Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Promenne
        /// </summary>

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Background b;
        public Postavicka me;

        public SpriteFont font;

        public int blockSize; //rozmer bloku
        int blockNumber; //pocet bloku (vyska)
        int WidthNoFS;
        int HeightNoFS;
        public int width; //rozmery okna v pixelech
        public int height;

        public bool InGame;
        public bool InMenu;
        public int level = 1;
        public Menu m;

        public Zoo zoo;
        /// <summary>
        /// Konstruktory
        /// </summary>

        public Game1()
        {
            WidthNoFS = 1000;
            HeightNoFS = 600;
            width = WidthNoFS; //rozmery okna v pixelech
            height = HeightNoFS;

            blockNumber = 6;
            blockSize = height / blockNumber;

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;


            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Metody
        /// </summary>       
        bool fullscreen = false;
        public void ToggleFS()
        {
            if (!fullscreen) //(!graphics.IsFullScreen)
            {

                height = graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                width = graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                blockSize = height / blockNumber;
                graphics.ToggleFullScreen();
                fullscreen = true;
            }
            else
            {
                graphics.ToggleFullScreen();
                graphics.PreferredBackBufferHeight = HeightNoFS;
                graphics.PreferredBackBufferWidth = WidthNoFS;
                graphics.ApplyChanges();
                height = HeightNoFS;
                width = WidthNoFS;
                blockSize = height / blockNumber;
                fullscreen = false;
            }
        }
        protected override void Initialize()
        {
            //camera = new Camera(GraphicsDevice.Viewport);
            // backgrounds = new Backgrounds[blockNumber];

            base.Initialize();
            IsMouseVisible = true;
            InMenu = true;
            InGame = false;

        }

        public void newgame()
        {
            LoadContent();
        }
        protected override void LoadContent()
        {

            m = new Menu(this);

            if (!InMenu)
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);

                Texture2D[][] texturyMe;
                texturyMe = new Texture2D[4][];
                texturyMe[0] = new Texture2D[2];
                texturyMe[1] = new Texture2D[2];
                texturyMe[2] = new Texture2D[3];
                texturyMe[3] = new Texture2D[3];

                texturyMe[0][0] = Content.Load<Texture2D>("Level " + level + "/forward1");
                texturyMe[1][0] = Content.Load<Texture2D>("Level " + level + "/backward1");
                texturyMe[2][0] = Content.Load<Texture2D>("Level " + level + "/AirFor");
                texturyMe[2][2] = Content.Load<Texture2D>("Level " + level + "/AirForHand");
                texturyMe[3][0] = Content.Load<Texture2D>("Level " + level + "/AirBack");
                texturyMe[3][2] = Content.Load<Texture2D>("Level " + level + "/AirBackHand");
                for (int i = 0; i < 4; i++)
                {
                    texturyMe[i][1] = Content.Load<Texture2D>("Level " + level + "/Head1");
                }
                font = Content.Load<SpriteFont>("font");

                int druhuKachlicek = 14;
                Texture2D[][] pozadi = new Texture2D[druhuKachlicek][];
                {
                    for (int i = 0; i < druhuKachlicek; i++)
                    {
                        pozadi[i] = new Texture2D[1];
                    }
                    pozadi[0] = new Texture2D[3];
                    pozadi[2] = new Texture2D[2];
                    for (int i = 0; i < pozadi.Length; i++)
                    {
                        for (int j = 0; j < pozadi[i].Length; j++)
                        {
                            pozadi[i][j] = Content.Load<Texture2D>("Level " + level + "/Bck1\\" + i + "-" + j);
                        }
                    }
                }
                zoo = new Zoo(this);
                b = new Background(this, pozadi, blockNumber, 300 * (width / blockSize));
                me = new Postavicka(this, texturyMe, 150 /*x*/ , 300 * (blockNumber - 1) /*y*/ , b);

            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (InGame && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                m.ktereMenu = KtereMenu.mainInGame;
                InMenu = true;
                IsMouseVisible = true;
            }
            if (InMenu)
            {
                m.update();
            }
            if (InGame && !InMenu)
            {
                me.update(gameTime);
                zoo.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (InMenu)
            {
                m.Draw(spriteBatch);
            }
            if (!InMenu && InGame)
            {
                b.draw(width, height, spriteBatch);
                zoo.Draw(spriteBatch);
                me.draw(spriteBatch);

            }

            //spriteBatch.DrawString(font, "GAME TIME = " + gameTime.TotalGameTime.Seconds + ":" + (gameTime.TotalGameTime.Milliseconds / 10), new Vector2(b.a/300 +25, 50), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}



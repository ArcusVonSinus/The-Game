﻿using System;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scrolling scrolling1;
        Scrolling scrolling2;
        Postavicka me;
        Postavicka alterego;
        Texture2D[] textury;
        private SpriteFont font;

        int blockSize; //rozmer bloku
        int blockNumber; //pocet bloku (vyska)
        int width;
        int height;





        public Game1()
        {
            blockNumber = 7;
            blockSize = 100;
            width = 1400;
            height = blockNumber * blockSize;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();


        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textury = new Texture2D[2];
            textury[0] = Content.Load<Texture2D>("forward");
            textury[1] = Content.Load<Texture2D>("backward");
            font = Content.Load<SpriteFont>("font");
            me = new Postavicka(textury, 0, 10, height, 100, 50, new Speed(0, 0));
            alterego = new Postavicka(textury, 0, 10, 200, 100, 50, new Speed(0, 0));

            scrolling1 = new Scrolling(Content.Load<Texture2D>(@"Backgrounds\images"), new Rectangle(0, 0, 800, 500));
            scrolling2 = new Scrolling(Content.Load<Texture2D>(@"Backgrounds\images2"), new Rectangle(800, 0, 800, 500));


            Background1 b = new Background1(8);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            me.update();
            alterego.update();
            
            // Scrolling Backgrounds
            if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
            if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;

            scrolling1.Update();
            scrolling2.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            
            //
            System.IO.StreamReader s = new System.IO.StreamReader(@"Content/l1.txt");
            spriteBatch.DrawString(font," " + s.ReadLine(), new Vector2(300, 300), Color.Black);
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);
            me.draw(spriteBatch);
            alterego.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);

            
        }
    }
}


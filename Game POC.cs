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

namespace WindowsGame3
{
   public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D fox;
        private SpriteFont arabicfont;
        private int score = 0;
        private AnimatedSprite postavicka;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("bckgrnd");
            fox = Content.Load<Texture2D>("fox");
            postavicka = new AnimatedSprite(Content.Load<Texture2D>("forward"), 2, 7);
            arabicfont = Content.Load<SpriteFont>("arabic"); 
        }

       
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

         protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            score++;
            postavicka.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            //spriteBatch.Draw(fox, new Vector2(10, 10), Color.White);
            spriteBatch.Draw(fox, new Rectangle(10, 10,90,150), Color.White);
            
            
            spriteBatch.DrawString(arabicfont, "Score " + score, new Vector2(200, 50), Color.Black);





            spriteBatch.End();
            postavicka.Draw(spriteBatch, new Vector2(100, 100),100);


            base.Draw(gameTime);
        }
    }
}

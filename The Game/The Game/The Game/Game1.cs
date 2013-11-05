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
       
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Postavicka me;
        Texture2D vzhledMe;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            base.Initialize();

        }

        protected override void LoadContent()
        {
           spriteBatch = new SpriteBatch(GraphicsDevice);
           vzhledMe = Content.Load<Texture2D>("forward");
           me = new Postavicka(new AnimatedSprite(vzhledMe, 2, 7), 10, 10, 50, 100);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            me.update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            me.draw(spriteBatch);



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

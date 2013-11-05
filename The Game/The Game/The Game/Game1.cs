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
        Texture2D[] textury;
        private SpriteFont font;
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
           textury = new Texture2D[2];
           textury[0] = Content.Load<Texture2D>("forward");
           textury[1] = Content.Load<Texture2D>("backward");
           font = Content.Load<SpriteFont>("font"); 
           me = new Postavicka(textury,0, 10, 10, 100, 50, new Speed(0, 0));
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
            //spriteBatch.DrawString(font," " + me.rychlost.x, new Vector2(300, 300), Color.Black);



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

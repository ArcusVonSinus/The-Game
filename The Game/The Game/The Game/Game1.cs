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
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /*Scrolling scrolling1;
        Scrolling scrolling2;*/
        //Backgrounds[] backgrounds;
        Background1 b;
        Postavicka me;        
        Texture2D[] textury;
        private SpriteFont font;

        int blockSize; //rozmer bloku
        int blockNumber; //pocet bloku (vyska)
        int width;
        int height;

        Camera camera;

        /// <summary>
        /// Konstruktory
        /// </summary>

        public Game1()
        {
            height = 804;
            width = 1474;

            blockNumber = 6;
            blockSize = height/blockNumber;  

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Metody
        /// </summary>

        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport);
           // backgrounds = new Backgrounds[blockNumber];
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textury = new Texture2D[4];
            textury[0] = Content.Load<Texture2D>("forward");
            textury[1] = Content.Load<Texture2D>("backward");
            textury[2] = Content.Load<Texture2D>("Air forw.");
            textury[3] = Content.Load<Texture2D>("Air back.");
            font = Content.Load<SpriteFont>("font");
            int druhukachlicek = 14;                
            Texture2D[][] pozadi = new Texture2D[druhukachlicek][];
            {
                for (int i = 0; i < druhukachlicek; i++)
                {
                    pozadi[i] = new Texture2D[1];
                }
                pozadi[0] = new Texture2D[3];
                pozadi[2] = new Texture2D[2];               
                for (int i = 0; i < pozadi.Length; i++)
                {
                    for (int j = 0; j < pozadi[i].Length; j++)
                    {
                        pozadi[i][j] = Content.Load<Texture2D>("Bck1\\" + i + "-" + j);
                    }
                }
            }
            b = new Background1(pozadi, blockNumber,300*(width/blockSize));

            me = new Postavicka(textury, 0, 10, 550, 70, 50, new Speed(0, 0),b);
            //scrolling1 = new Scrolling(Content.Load<Texture2D>(@"Backgrounds\les"), new Rectangle(0, 0, 1400, 700));
            //scrolling2 = new Scrolling(Content.Load<Texture2D>(@"Backgrounds\les"), new Rectangle(1400, 0, 1400, 700));

           // backgrounds[0] = new Backgrounds(Content.Load<Texture2D>(@"Backgrounds\les"), new Rectangle(-1400, 0, 1400, 700));
           // backgrounds[1] = new Backgrounds(Content.Load<Texture2D>(@"Backgrounds\les"), new Rectangle(0, 0, 1400, 700));
           // backgrounds[2] = new Backgrounds(Content.Load<Texture2D>(@"Backgrounds\les"), new Rectangle(1400, 0, 1400, 700));

            //Background1 b = new Background1(8);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            me.update(gameTime);            
            
            // Scrolling Backgrounds
            //if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
            //    scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
            //if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
            //    scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
            //scrolling1.Update();
            //scrolling2.Update(); 

           /* if (camera.centre.X <= backgrounds[1].rectangle.X)
            {
                backgrounds[2].rectangle.X -= 4200;
                VycentrujLevy(backgrounds);
            }
            if (camera.centre.X > backgrounds[2].rectangle.X)
            {
                backgrounds[0].rectangle.X += 4200;
                VycentrujPravy(backgrounds);
            }

            camera.Update(gameTime, me);*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,null,null,null,null,camera.transform);
            spriteBatch.Begin();          
           // System.IO.StreamReader s = new System.IO.StreamReader(@"Content/l1.txt");
           
            //scrolling1.Draw(spriteBatch);
            //scrolling2.Draw(spriteBatch);
          /*  for (int i = 0; i < 3; i++)
            {
                backgrounds[i].Draw(spriteBatch);
            }*/
            b.draw(width, height, spriteBatch);
            me.draw(spriteBatch);
            //spriteBatch.DrawString(font," " + gameTime.TotalGameTime.Milliseconds, new Vector2(300, 300), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);            
        }

       /* public void VycentrujLevy<T>(T[] policko)
        {
            if (policko.Length == 3)
            {
                T pom = policko[0];
                policko[0] = policko[2];
                policko[2] = policko[1];
                policko[1] = pom;
            }
        }
        public void VycentrujPravy<T>(T[] policko)
        {
            if (policko.Length == 3)
            {
                T pom = policko[0];
                policko[0] = policko[1];
                policko[1] = policko[2];
                policko[2] = pom;
            }
        }*/
    }
}



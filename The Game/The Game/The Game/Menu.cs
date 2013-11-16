using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace The_Game
{
    public enum KtereMenu { main, chooseLevel, highscores }
    class Event
    {

    }
    class Button
    {
        Menu menu;
        int buttonNo;
        KtereMenu ktereMenu;
        Texture2D[] vzhled;
        int vzhledNo;       
        Rectangle position;
        MouseState mouseState;
        bool isMouseOver()
        {
            int x = mouseState.X;
            int y = mouseState.Y;
            Rectangle temp;
            temp = new Rectangle(x, y, 1,1);
            return (temp.Intersects(position));
        }
        public Button(Menu parent, int buttonNo, KtereMenu ktereMenu,string name)
        {
            this.menu = parent;
            this.buttonNo=buttonNo;
            this.ktereMenu = ktereMenu;
            vzhled = new Texture2D[3];
            for(int i = 0;i<3;i++)
            {
                vzhled[i] = menu.game.Content.Load<Texture2D>("Menu/Butt" + name + i);
            }
            vzhledNo = 0;
        }
        public void Update()
        {
            position = new Rectangle(menu.buttonsX, menu.buttonsY + (int)(1.5f*buttonNo * menu.buttonSizeH * menu.zmenseni), (int)(menu.buttonSizeW * menu.zmenseni), (int)(menu.buttonSizeH * menu.zmenseni));
            mouseState = Mouse.GetState();
            if(menu.ktereMenu == ktereMenu)
            {
                if (!isMouseOver())
                {
                    vzhledNo = 0;
                }
                else
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        vzhledNo = 2;
                    }
                    else
                    {
                        if (vzhledNo == 2)
                        {
                            menu.Clicked(buttonNo);
                            vzhledNo = 1;
                        }
                        else
                        {
                            vzhledNo = 1;
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (ktereMenu == menu.ktereMenu)
            {
                spriteBatch.Draw(vzhled[vzhledNo], position, Color.White);
            }
        }
        

    }
    class Menu //menu je velke 1500x1000, pak se prepocita
    {
        public KtereMenu ktereMenu;

        public float zmenseni{
            get
            {
                float a = (game.width - 100) / 1000f;
                if ((game.height - 100) / 1500f < a)
                    return (game.height - 100) / 1500f;
                else
                    return a;
            }
            }
                

       
        public int buttonSizeW = 800,buttonSizeH = 100; //1500x1000
        public int buttonsX, buttonsY;
        Button[] tlacitka;
        public Game1 game;
        Texture2D pozadi,pozadiMenu;
        public Menu(Game1 game)
        {
            this.game = game;            
            ktereMenu = KtereMenu.main;

            pozadi = game.Content.Load<Texture2D>("Menu/pozadi");
            pozadiMenu = game.Content.Load<Texture2D>("Menu/pozadiMenu");

            tlacitka = new Button[2];
            //----------------------------------------------
            KtereMenu temp = KtereMenu.main;
            tlacitka[0] = new Button(this, 0, temp, "NewGame");
            tlacitka[1] = new Button(this, 1, temp, "NewGame");
            /*
            tlacitka[1] = new Button(this, 1, temp,"ChooseLevel");
            tlacitka[2] = new Button(this, 2, temp,"Highscores");
            // tlacitka = */
        }
        public void update()
        {
            foreach (Button tl in tlacitka)
            {
                tl.Update();
            }
        }
        public void Clicked(int buttonNo)
        {
            if (ktereMenu == KtereMenu.main)
            {
                if (buttonNo == 0)
                {
                    game.InMenu = false;
                    game.level = 1;
                    game.InGame = true;
                    game.newgame();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(pozadi, new Rectangle(0,0,game.width,game.height), Color.White);
            spriteBatch.Draw(pozadiMenu,new Rectangle(50+(int)(game.width-zmenseni*1000-100)/2, 50+(int)(game.height-zmenseni*1500-100)/2, (int)(zmenseni*1000),  (int)(zmenseni*1500) ),Color.White);
            buttonsX = 50 + (int)(game.width - zmenseni * 1000 - 100) / 2;
            buttonsX += (int)(zmenseni * (1000 - buttonSizeW)) / 2;
            buttonsY = 50 + (int)(game.height - zmenseni * 1500 - 100) / 2;
            buttonsY += (int)(zmenseni * 300);
            foreach (Button tl in tlacitka)
            {
               
                tl.Draw(spriteBatch);
            }
                       
        }
    }
}

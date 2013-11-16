using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        Vector2 pozice{
            get
            {
                Vector2 v = new Vector2();
                v.X = menu.buttonsX;
                v.Y = menu.buttonsY + 1.5f*buttonNo*menu.buttonSizeH;
                return v;
            }
        }
        Vector2 size{
            get
            {
                Vector2 v = new Vector2();
                v.X = menu.buttonSizeW;
                v.Y = menu.buttonSizeH;
                return v;
            }
        }
        bool isMouseOver()
        {

        }
        public Button(Menu parent, int buttonNo, KtereMenu ktereMenu,Texture2D[] vzhled)
        {
            this.menu = parent;
            this.buttonNo=buttonNo;
            this.ktereMenu = ktereMenu;
            this.vzhled = vzhled;
        }
        public Update()
        {
            if(menu.ktereMenu == ktereMenu)
            {

            }
        }
        

    }
    class Menu
    {
        public KtereMenu ktereMenu;

                
        float buttonRelSizeW = 0.5f;

        public int buttonSizeW
        {
            get
            {
                return((int) (game.width*buttonRelSizeW)); 
            }
        }
        public int buttonSizeH;
        public int buttonsX,buttonsY;
        Button[] tlacitka;
        Game1 game;
        public Menu(Game1 game)
        {
            this.game = game;            
            ktereMenu = KtereMenu.main;

            tlacitka = new Button[4];
            //----------------------------------------------
            KtereMenu temp = KtereMenu.main;
            tlacitka[0] = new Button(this, 0, temp);
            tlacitka[1] = new Button(this, 0, temp);
            tlacitka[2] = new Button(this, 0, temp);
            // tlacitka = 
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

        }
        public void Draw()
        {
            foreach (Button tl in tlacitka)
            {
                tl.Draw();
            }
        }
    }
}

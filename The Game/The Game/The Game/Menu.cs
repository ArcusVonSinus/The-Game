using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace The_Game
{
    public class Text
    {
        public string text;
        string font;
        int height,x,y;
        SpriteFont pismo;
        float size = 0f;
        Game1 game;
        bool zarovnatdoprava = false;
        void LoadFont()
        {
            float sizetemp = 0.435f * height;
            if (sizetemp != size)
            {
                size = sizetemp;
                if (size < 21.7)
                {
                    pismo = game.Content.Load<SpriteFont>(@"Fonts/" + font + "15");
                }
                else
                {
                    pismo = game.Content.Load<SpriteFont>(@"Fonts/" + font + "29");
                }
                
            }
        }
        public Text(Game1 game, int x, int y, int height, string text)
        {
            this.game = game; this.x = x; this.y = y; this.height = height; this.text = text; this.font = "font";
        }
        public Text(Game1 game, int x, int y, int height, string text, bool zarovnatdoprava)
        {
            this.zarovnatdoprava = zarovnatdoprava;
            this.game = game; this.x = x; this.y = y; this.height = height; this.text = text; this.font = "font";
        }
        public Text(Game1 game, int x, int y, int height,string text, string font)
        {
            this.game = game; this.x = x; this.y = y; this.height = height; this.text = text; this.font = font;
        }
        public Text(Game1 game, int x, int y, int height, string text, string font,bool zarovnatdoprava)
        {
            this.zarovnatdoprava = zarovnatdoprava;
            this.game = game; this.x = x; this.y = y; this.height = height; this.text = text; this.font = font;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            LoadFont();
            if(zarovnatdoprava)
                spriteBatch.DrawString(pismo, text, new Vector2(x - pismo.MeasureString(text).X,y), Color.Black);
            else
                spriteBatch.DrawString(pismo, text, new Vector2(x, y), Color.Black);
        }
        public void ChangeLoc(Vector2 v,int height)
        {
            this.height = height;
            x = (int) v.X;
            y = (int) v.Y;
        }
        public void ChangeText(string text)
        {
            this.text = text;
        }
    }
    public enum KtereMenu { main, mainInGame, chooseLevel, chooseLevelInGame, settings,settingsInGame, highscores, zadaniJmena}
    class Label
    {
        protected Menu menu;
        int labelNo;
        protected KtereMenu ktereMenu;
        Texture2D vzhled;
        Rectangle texturePosition;
        public Text textscore;
        public Text textname;
        public Label()
        {

        }
        public Label(Menu parent, int labelNo, KtereMenu ktereMenu, string score, string name)
        {
            this.menu = parent;
            this.labelNo = labelNo;
            this.ktereMenu = ktereMenu;
            textscore = new Text(parent.game, 0, 0, (int) (1f*parent.buttonSizeH*parent.zmenseni), score,true);
            textname = new Text(parent.game, 0, 0, (int)(1f * parent.buttonSizeH * parent.zmenseni), name);
            vzhled = menu.game.Content.Load<Texture2D>("Menu/Labels/label");
        }
        public virtual void Update()
        {
            textscore.ChangeLoc(new Vector2(menu.buttonsX + (int)(menu.buttonSizeW * menu.zmenseni / 4), menu.buttonsY + (int)(1.5f * labelNo * menu.buttonSizeH * menu.zmenseni)),(int) (1f*menu.buttonSizeH*menu.zmenseni));
            textname.ChangeLoc(new Vector2(menu.buttonsX + (int)(menu.buttonSizeW * menu.zmenseni / 3), menu.buttonsY + (int)(1.5f * labelNo * menu.buttonSizeH * menu.zmenseni)), (int)(1f * menu.buttonSizeH * menu.zmenseni));
            texturePosition = new Rectangle(menu.buttonsX, menu.buttonsY + (int)(1.5f * labelNo * menu.buttonSizeH * menu.zmenseni), (int)(menu.buttonSizeW * menu.zmenseni), (int)(menu.buttonSizeH * menu.zmenseni));           
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ktereMenu == menu.ktereMenu)
            {
                int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;                
                spriteBatch.Draw(vzhled, texturePosition, Color.White);
                textname.Draw(spriteBatch);
                textscore.Draw(spriteBatch);
            }
        }        
    }
    class HighScoreBackground : Label
    {
        public const float okraj = 45f;
        Texture2D vzhled;
        Rectangle texturePosition;
        public HighScoreBackground(Menu parent, KtereMenu ktereMenu)
        {            
            this.menu = parent;
            this.ktereMenu = ktereMenu;
            vzhled = menu.game.Content.Load<Texture2D>("Menu/Labels/HighScoreBackground");            
        }
        public override void Update()
        {
            texturePosition = new Rectangle(
                menu.buttonsX - (int)(okraj * menu.zmenseni), 
                menu.buttonsY - (int)(okraj * menu.zmenseni), 
                (int)((menu.buttonSizeW + 2 * okraj) * menu.zmenseni), 
                (int)(9 * menu.buttonSizeH * menu.zmenseni + okraj * menu.zmenseni)
                );
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ktereMenu == menu.ktereMenu)
                spriteBatch.Draw(vzhled, texturePosition, Color.White);
        }
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
        string name;
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
            this.name = name;
            this.menu = parent;
            this.buttonNo = buttonNo;
            this.ktereMenu = ktereMenu;
            vzhled = new Texture2D[3];
            for(int i = 0; i < 3; i++)
            {
                vzhled[i] = menu.game.Content.Load<Texture2D>("Menu/Buttons/Butt" + name + i);
            }
            vzhledNo = 0;
        }        
        public void Update()
        {
            position = new Rectangle(
                menu.buttonsX, 
                menu.buttonsY + (int)(1.5f*buttonNo * menu.buttonSizeH * menu.zmenseni), 
                (int)(menu.buttonSizeW * menu.zmenseni), 
                (int)(menu.buttonSizeH * menu.zmenseni));
            mouseState = Mouse.GetState();
            if(menu.ktereMenu == ktereMenu)
            {
                if (!isMouseOver())
                {
                    if (menu.vybranaPolozka == buttonNo||(ktereMenu == KtereMenu.highscores && menu.vybranaPolozka == 0))
                    {
                        vzhledNo = 1;
                    }
                    else
                    {
                        vzhledNo = 0;
                    }
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
                            menu.Clicked(buttonNo,name);
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
    public class Menu //menu je velke 1500x1000, pak se prepocita
    {
        public KtereMenu ktereMenu, temp;

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
        public int vybranaPolozka; // pro ovladani klavesnici
        public int buttonSizeW = 800,buttonSizeH = 100; //1500x1000
        public int buttonsX, buttonsY;
        public int polozekHS = 6;
        Button[] tlacitka;
        Label[] stitky;
        Label textovePole;
        public  int scorekzapsani = 0;
        HighScoreBackground pozadiHS;
        public Game1 game;
        Texture2D pozadi,pozadiMenu;
        public Menu(Game1 game)
        {

            this.game = game;            
            ktereMenu = KtereMenu.main;
            vybranaPolozka = 0;
            pozadi = game.Content.Load<Texture2D>("Menu/pozadi");
            pozadiMenu = game.Content.Load<Texture2D>("Menu/pozadiMenu");

            tlacitka = new Button[26];
            stitky = new Label[polozekHS];            
            createLabels();

            textovePole =   new Label(this, 0, KtereMenu.zadaniJmena, "0", "a"); 
            //-----------------------------------------------------------------
            temp = KtereMenu.main;
            tlacitka[0] = new Button(this, 0, temp, "NewGame");
            tlacitka[1] = new Button(this, 1, temp, "ChooseLevel");
            tlacitka[2] = new Button(this, 2, temp, "Highscores");
            tlacitka[3] = new Button(this, 3, temp, "Settings");
            tlacitka[4] = new Button(this, 4, temp, "Quit");
            //----------------------------------------------------------------------
            temp = KtereMenu.mainInGame;
            tlacitka[5] = new Button(this, 0, temp, "Continue");
            tlacitka[6] = new Button(this, 1, temp, "NewGame");
            tlacitka[7] = new Button(this, 2, temp, "ChooseLevel");
            tlacitka[8] = new Button(this, 3, temp, "Settings");
            tlacitka[9] = new Button(this, 4, temp, "Quit");
            //----------------------------------------------------------------------
            temp = KtereMenu.settings;
            tlacitka[10] = new Button(this, 0, temp, "ToggleFullscreen");
            tlacitka[21] = new Button(this, 1, temp, "Music");
            tlacitka[22] = new Button(this, 2, temp, "SoundEffects");
            tlacitka[11] = new Button(this, 3, temp, "Back");
            //----------------------------------------------------------------------
            temp = KtereMenu.settingsInGame;
            tlacitka[12] = new Button(this, 0, temp, "ToggleFullscreen");
            tlacitka[19] = new Button(this, 1, temp, "Music");
            tlacitka[20] = new Button(this, 2, temp, "SoundEffects");
            tlacitka[13] = new Button(this, 3, temp, "Back");
            //----------------------------------------------------------------------
            temp = KtereMenu.chooseLevel;
            tlacitka[14] = new Button(this, 0, temp, "Level1");
            tlacitka[23] = new Button(this, 1, temp, "Level2");
            tlacitka[15] = new Button(this, 2, temp, "Back");
            //----------------------------------------------------------------------
            temp = KtereMenu.chooseLevelInGame;
            tlacitka[16] = new Button(this, 0, temp, "Level1");
            tlacitka[24] = new Button(this, 1, temp, "Level2");
            tlacitka[17] = new Button(this, 2, temp, "Back");
            //----------------------------------------------------------------------
            temp = KtereMenu.highscores;
            pozadiHS = new HighScoreBackground(this, temp);
            tlacitka[18] = new Button(this, 6, temp, "Back");
            //--------------------------------------------------------------------
            temp = KtereMenu.zadaniJmena;            
            tlacitka[25] = new Button(this, 6, temp, "OK");
            //--------------------------------------------------------------------


            pressed = false;
        }
        bool pressed = true;  //pro ovladani klavesnici
        bool pressAbc = true;
        void createLabels()
        {
            System.IO.StreamReader HSReader = new System.IO.StreamReader(@"Content/HS.txt");
            for (int i = 0; i < polozekHS; i++)
            {

                stitky[i] = new Label(this, i, KtereMenu.highscores, HSReader.ReadLine(), HSReader.ReadLine());
            }
            HSReader.Close();
            /*stitky[1] = new Label(this, 1, KtereMenu.highscores, "42.467", "Karla");
            stitky[2] = new Label(this, 2, KtereMenu.highscores, "37.189", "Honza");
            stitky[3] = new Label(this, 3, KtereMenu.highscores, "31.112", "Lojza");
            stitky[4] = new Label(this, 4, KtereMenu.highscores, "29.215", "Tom");
            stitky[5] = new Label(this, 5, KtereMenu.highscores, "28.623", "Elen");*/

        }

        bool readingname;
        StringBuilder namereader;
        void ReadName()
        {
            if (!readingname)
            {
                readingname = true;
                namereader = new StringBuilder();
                textovePole.textscore.text = scorekzapsani.ToString();              
            }
            if (pressAbc)
            {
                Keys[] temp = Keyboard.GetState().GetPressedKeys();
                if (temp.Length == 0)
                {
                    pressAbc = false;
                }
            }
            else if (!pressAbc)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    namereader.Append("a");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    namereader.Append("b");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                {
                    namereader.Append("c");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    namereader.Append("d");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    namereader.Append("e");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.F))
                {
                    namereader.Append("f");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.G))
                {
                    namereader.Append("g");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.H))
                {
                    namereader.Append("h");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    namereader.Append("i");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.J))
                {
                    namereader.Append("j");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    namereader.Append("k");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    namereader.Append("l");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    namereader.Append("m");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    namereader.Append("n");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    namereader.Append("o");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    namereader.Append("p");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    namereader.Append("q");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    namereader.Append("r");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    namereader.Append("s");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    namereader.Append("t");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    namereader.Append("u");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.V))
                {
                    namereader.Append("v");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    namereader.Append("w");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    namereader.Append("x");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    namereader.Append("y");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    namereader.Append("z");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    namereader.Append(" ");
                    pressAbc = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    namereader.Remove(namereader.Length - 1, 1);
                    pressAbc = true;
                }
            }

            if(namereader.Length == 1 && namereader[0]>='a')
            {
                namereader[0]^=' ';
            }
            textovePole.textname.text = namereader.ToString();
        }
        public void update()
        {
            if (ktereMenu == KtereMenu.zadaniJmena)
            {
                ReadName();
            }
            if (!pressed && Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                pressed = true;
                vybranaPolozka++;
                int temp=0;
                switch (ktereMenu)
                {
                    case KtereMenu.main:
                        temp = 5;
                        break;
                    case KtereMenu.mainInGame:
                        temp= 5;
                        break;
                    case KtereMenu.settings:
                        temp = 4;
                        break;
                    case KtereMenu.settingsInGame:
                        temp = 4;
                        break;
                    case KtereMenu.chooseLevel:
                        temp = 3;
                        break;
                    case KtereMenu.chooseLevelInGame:
                        temp = 3;
                        break;
                    case KtereMenu.highscores:

                        temp = 1;
                        break;
                    case KtereMenu.zadaniJmena:

                        temp = 1;
                        break;
                }
                if(vybranaPolozka >= temp)
                    vybranaPolozka = temp-1;
            }
            if (!pressed && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                pressed = true;
                if (vybranaPolozka>0)
                    vybranaPolozka--;
                
            }
            if (!pressed && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                pressed = true;
                if (ktereMenu == KtereMenu.highscores)
                {
                    Clicked(6, "");
                }
                else if (ktereMenu == KtereMenu.zadaniJmena)
                {
                    Clicked(6, "");
                }
                else
                    Clicked(vybranaPolozka, "");
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && 
                Keyboard.GetState().IsKeyUp(Keys.Down) && 
                Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                pressed = false;
            }

            if (ktereMenu == KtereMenu.highscores) pozadiHS.Update();
            foreach (Button tl in tlacitka)
            {
                tl.Update();
            }
            foreach (Label st in stitky)
            {
                st.Update();
            }
            textovePole.Update();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        void NewGame(int level)
        {
            game.InMenu = false;
            game.IsMouseVisible = false;
            game.level = level;
            game.InGame = true;
            game.newgame();
        }
        void zapisscore()
        {
            int polozekHS = game.m.polozekHS;
            int[] scorre = new int[polozekHS];
            string[] names = new string[polozekHS];
            System.IO.StreamReader HSReader = new System.IO.StreamReader(@"Content/HS.txt");
            for (int i = 0; i < game.m.polozekHS; i++)
            {
                scorre[i] = Int32.Parse(HSReader.ReadLine());
                names[i] = HSReader.ReadLine();
            }
            HSReader.Close();



            string name = namereader.ToString();
            scorre[polozekHS - 1] = scorekzapsani;
            names[polozekHS - 1] = name;
            for (int i = game.m.polozekHS - 1; i >= 1; i--)
            {
                if (scorre[i] >= scorre[i - 1])
                {
                    int temp = scorre[i];
                    scorre[i] = scorre[i - 1];
                    scorre[i - 1] = temp;
                    //------
                    string tem = names[i];
                    names[i] = names[i - 1];
                    names[i - 1] = tem;
                }
            }
            System.IO.StreamWriter HSWriter = new System.IO.StreamWriter(@"Content/HS.txt");
            for (int i = 0; i < polozekHS; i++)
            {
                HSWriter.WriteLine(scorre[i]);
                HSWriter.WriteLine(names[i]);
            }
            HSWriter.Close();
        }
        public void Clicked(int buttonNo,string name)
        {
            vybranaPolozka = 0;
            if (name == "Quit")
            {
                MediaPlayer.Stop();
                game.Exit();
                return;
            }
            
            if (ktereMenu == KtereMenu.main)
            {
                if (buttonNo == 0)
                {
                    NewGame(1);
                    return;
                }
                if (buttonNo == 1)
                {
                    ktereMenu = KtereMenu.chooseLevel;
                    return;
                }
                if (buttonNo == 2)
                {
                    createLabels();
                    ktereMenu = KtereMenu.highscores;
                    return;
                }
                if (buttonNo == 3)
                {
                    ktereMenu = KtereMenu.settings;
                    return;
                }
                if (buttonNo == 4)
                {
                    MediaPlayer.Stop();
                    game.Exit();
                    return;
                }
            }
            if (ktereMenu == KtereMenu.mainInGame)
            {
                if (buttonNo == 0)
                {                    
                    game.InMenu = false;
                    game.IsMouseVisible = false;
                }
                if (buttonNo == 1)
                {
                    NewGame(1);
                    return;
                }
                if (buttonNo == 2)
                {
                    ktereMenu = KtereMenu.chooseLevelInGame;
                    return;
                }
                if (buttonNo == 3)
                {
                    ktereMenu = KtereMenu.settingsInGame;
                    return;
                }
                if (buttonNo == 4)
                {
                    MediaPlayer.Stop();
                    game.Exit();
                    return;
                }
            }
            if (ktereMenu == KtereMenu.settings)
            {
                if (buttonNo == 0)
                {
                    game.ToggleFS();
                    return;
                }
                if (buttonNo == 1)
                {
                    if (MediaPlayer.Volume == 1)
                        MediaPlayer.Volume = 0;
                    else
                        MediaPlayer.Volume = 1;
                    return;
                }
                if (buttonNo == 2)
                {
                    if (SoundEffect.MasterVolume == 1)
                        SoundEffect.MasterVolume = 0;
                    else
                        SoundEffect.MasterVolume = 1;
                    return;
                }
                if (buttonNo == 3)
                {
                    ktereMenu = KtereMenu.main;
                    return;
                }
            }
            if (ktereMenu == KtereMenu.settingsInGame)
            {
                if (buttonNo == 0)
                {
                    game.ToggleFS();
                    return;
                }
                if (buttonNo == 1)
                {
                    if (MediaPlayer.Volume == 1)
                        MediaPlayer.Volume = 0;
                    else
                        MediaPlayer.Volume = 1;
                    return;
                }
                if (buttonNo == 2)
                {
                    if (SoundEffect.MasterVolume == 1)
                        SoundEffect.MasterVolume = 0;
                    else
                        SoundEffect.MasterVolume = 1;
                    return;
                }
                if (buttonNo == 3)
                {
                    ktereMenu = KtereMenu.mainInGame;
                    return;
                }
            }
            if (ktereMenu == KtereMenu.highscores)
            {
                
                if (buttonNo == 6)
                {
                    ktereMenu = KtereMenu.main;
                    return;
                }
            }
            if (ktereMenu == KtereMenu.chooseLevel)
            {
                if (buttonNo == 0)
                {
                    NewGame(1);
                    return;
                }
                if (buttonNo == 1)
                {
                    NewGame(2);
                    return;
                }
                if (buttonNo == 2)
                {
                    ktereMenu = KtereMenu.main;
                    return;
                }
            }
            if (ktereMenu == KtereMenu.chooseLevelInGame)
            {
                if (buttonNo == 0)
                {
                    NewGame(1);
                    return;
                }
                if (buttonNo == 1)
                {
                    NewGame(2);
                    return;
                }
                if (buttonNo == 2)
                {
                    ktereMenu = KtereMenu.mainInGame;
                    return;
                }
            }
            if (ktereMenu == KtereMenu.zadaniJmena)
            {
                if (buttonNo == 6&&namereader.Length>0)
                {
                    zapisscore();
                    createLabels();
                    ktereMenu = KtereMenu.highscores;
                    return;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(pozadi, new Rectangle(0,0,game.width,game.height), Color.White);
            spriteBatch.Draw(
                pozadiMenu,
                new Rectangle(
                    50 + (int)(game.width - zmenseni * 1000 - 100) / 2,
                    50 + (int)(game.height - zmenseni * 1500 - 100) / 2,
                    (int)(zmenseni * 1000), (int)(zmenseni * 1500)),
                Color.White);
            buttonsX = 50 + (int)(game.width - zmenseni * 1000 - 100) / 2;
            buttonsX += (int)(zmenseni * (1000 - buttonSizeW)) / 2;
            buttonsY = 50 + (int)(game.height - zmenseni * 1500 - 100) / 2;
            buttonsY += (int)(zmenseni * 320);
            pozadiHS.Draw(spriteBatch);
            foreach (Button tl in tlacitka)
            {  
                tl.Draw(spriteBatch);
            }
            foreach (Label st in stitky)
            {
                st.Draw(spriteBatch);
            }
            textovePole.Draw(spriteBatch);
        }
    }
}

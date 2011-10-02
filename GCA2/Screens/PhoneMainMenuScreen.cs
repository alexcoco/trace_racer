#region File Description
//-----------------------------------------------------------------------------
// PhoneMainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace GCA2
{
    class PhoneMainMenuScreen : PhoneMenuScreen
    {
        BackgroundScreen background;
        int currentMenuPage = 0;
        public PhoneMainMenuScreen()
            : base("")
        {

        }


        void play()
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }

        void help()
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                if (screen.GetType() == typeof(BackgroundScreen))
                {
                    background = (BackgroundScreen)screen;
                    background.BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("HelpScreen");
                }
            }
        }
        void credits()
        {

            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                if (screen.GetType() == typeof(BackgroundScreen))
                {
                    background = (BackgroundScreen)screen;
                    background.BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("CreditsScreen");
                }
            }
        }

        private void backToMenu()
        {
            MenuButtons.Clear();
            ScreenManager.RemoveScreen(background);
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(this, null);
            this.Activate(true);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                Vector2 touchposition = tl.Position;
                if (currentMenuPage == 0)
                {
                    if ((touchposition.X >= 0 && touchposition.X <= 200) && (touchposition.Y >= 420 && touchposition.Y <= 480))
                    {
                        play();
                    }
                    else if ((touchposition.X >= 600 && touchposition.X <= 800) && (touchposition.Y >= 420 && touchposition.Y <= 480))
                    {
                        help();
                        currentMenuPage = 1;
                    }
                    else if ((touchposition.X >= 725 && touchposition.X <= 800) && (touchposition.Y >= 0 && touchposition.Y <= 45))
                    {
                        credits();
                        currentMenuPage = 2;
                    }
                }
                else if (currentMenuPage == 1)
                {
                    if ((touchposition.X >= 0 && touchposition.X <= 100) && (touchposition.Y >= 0 && touchposition.Y <= 45))
                    {
                        backToMenu();
                        currentMenuPage = 0;
                    }

                }
                else if (currentMenuPage == 2)
                {
                    if ((touchposition.X >= 0 && touchposition.X <= 100) && (touchposition.Y >= 0 && touchposition.Y <= 45))
                    {
                        backToMenu();
                        currentMenuPage = 0;
                    }
                }
            }
        }
    }
}

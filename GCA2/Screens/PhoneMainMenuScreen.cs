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
        int currentHelpImage = 0;
        BackgroundScreen background;
        public PhoneMainMenuScreen()
            : base("Main Menu")
        {

        }
        
 
        void play()
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }
      
        void help()
        {
            MenuButtons.Clear();
            this.menuTitle = "Help";
            setUpHelpButtons();
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                if (screen.GetType() == typeof(BackgroundScreen))
                {
                    background = (BackgroundScreen)screen;
                    background.BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("sprites/helpImage_" + 0);
                }
            }   
        }
        private void setUpHelpButtons()
        {
            Button menuButton = new Button("Menu");
            menuButton.Tapped += menuButton_Tapped;
            menuButton.Position.X = (TouchPanel.DisplayWidth / 2) - menuButton.Size.X / 2;
            menuButton.Position.Y = TouchPanel.DisplayHeight - 100;
            MenuButtons.Add(menuButton);

            Button prevButton = new Button("Prev");
            prevButton.Tapped += prevButton_Tapped;
            prevButton.Position.X = 0;
            prevButton.Position.Y = TouchPanel.DisplayHeight - 100;
            MenuButtons.Add(prevButton);
 
            Button nextButton = new Button("Next");
            nextButton.Tapped += nextButton_Tapped;
            nextButton.Position.X = TouchPanel.DisplayWidth - menuButton.Size.X;
            nextButton.Position.Y = TouchPanel.DisplayHeight - 100;
            MenuButtons.Add(nextButton);
        }

        void nextButton_Tapped(object sender, EventArgs e)
        {
            if (currentHelpImage < 3)
            {
                currentHelpImage++;
                foreach (GameScreen screen in ScreenManager.GetScreens())
                {
                    if (screen.GetType() == typeof(BackgroundScreen))
                    {
                         background = (BackgroundScreen)screen;
                        background.BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("sprites/helpImage_" + currentHelpImage);
                    }
                }
            }
        }
        void prevButton_Tapped(object sender, EventArgs e)
        {
            if (currentHelpImage != 0)
            {
                currentHelpImage--;
                foreach (GameScreen screen in ScreenManager.GetScreens())
                {
                    if (screen.GetType() == typeof(BackgroundScreen))
                    {
                        background = (BackgroundScreen)screen;
                        background.BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("sprites/helpImage_" + currentHelpImage);
                    }
                }   
            }
        }
        void menuButton_Tapped(object sender, EventArgs e)
        {
            MenuButtons.Clear();
            ScreenManager.RemoveScreen(background);
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(this, null);
            this.menuTitle = "Main Menu";
            this.Activate(true);

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                Vector2 touchposition = tl.Position;
                if ((touchposition.X >= 0 && touchposition.X <= 200)&& (touchposition.Y >= 420 && touchposition.Y <= 480))
                {
                    play();
                }
                if ((touchposition.X >= 600 && touchposition.X <= 800) && (touchposition.Y >= 420 && touchposition.Y <= 480))
                {
                    help();
                }
            }
        }
    }
}

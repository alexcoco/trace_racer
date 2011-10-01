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
            setUpMenuButtons();
        }

        private void setUpMenuButtons()
        {
            // Create a button to start the game
            Button playButton = new Button("Play");
            playButton.Tapped += playButton_Tapped;
            MenuButtons.Add(playButton);

            BooleanButton musicButton = new BooleanButton("Music", true);
            musicButton.Tapped += musicButton_Tapped;
            MenuButtons.Add(musicButton);

            Button helpButton = new Button("HELP");
            helpButton.Tapped += helpButton_Tapped;
            MenuButtons.Add(helpButton);

            Button quitButton = new Button("Quit");
            quitButton.Tapped += quitButton_Tapped;
            MenuButtons.Add(quitButton);
        }
        
        void playButton_Tapped(object sender, EventArgs e)
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }
        void musicButton_Tapped(object sender, EventArgs e)
        {
            BooleanButton button = sender as BooleanButton;

            // In a real game, you'd want to store away the value of 
            // the button to turn off music here. :)
        }

        void helpButton_Tapped(object sender, EventArgs e)
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
            setUpMenuButtons();
            this.menuTitle = "Main Menu";
            this.Activate(true);

        }

        void quitButton_Tapped(object sender, EventArgs e)
        {
            OnCancel();
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
            base.OnCancel();
        }
    }
}

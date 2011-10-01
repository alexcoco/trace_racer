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
using GCA2.Screens;

namespace GCA2
{
    class PhoneMainMenuScreen : PhoneMenuScreen
    {
        public PhoneMainMenuScreen()
            : base("Main Menu")
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
            //TODO ADD HELP MENU SCREEN!!!
            HelpScreen.Load(ScreenManager, false, PlayerIndex.One);
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

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

        public PhoneMainMenuScreen(BackgroundScreen background)
            : base("")
        {
            this.background = background;
        }


        private void play()
        {
            // When the "Play" button is tapped, we load the GameplayScreen
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
        }

        private void help(GameTime gameTime)
        {
            background.position = 1;
            background.Draw(gameTime);
             
        }
        private void credits(GameTime gameTime)
        {

            background.position = 2;
            background.Draw(gameTime);
        }

        private void backToMenu(GameTime gameTime)
        {
            background.position = 0;
            background.Draw(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            background.Draw(gameTime);
            base.Draw(gameTime);

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
                        help(gameTime);
                        currentMenuPage = 1;
                    }
                    else if ((touchposition.X >= 675 && touchposition.X <= 800) && (touchposition.Y >= 0 && touchposition.Y <= 100))
                    {
                        credits(gameTime);
                        currentMenuPage = 2;
                    }
                }
                else if (currentMenuPage == 1)
                {
                    if ((touchposition.X >= 0 && touchposition.X <= 100) && (touchposition.Y >= 0 && touchposition.Y <= 100))
                    {
                        backToMenu(gameTime);
                        currentMenuPage = 0;
                    }

                }
                else if (currentMenuPage == 2)
                {
                    if ((touchposition.X >= 0 && touchposition.X <= 100) && (touchposition.Y >= 0 && touchposition.Y <= 100))
                    {
                        backToMenu(gameTime);
                        currentMenuPage = 0;
                    }
                }
            }
        }
    }
}

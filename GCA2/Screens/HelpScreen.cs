using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace GCA2.Screens
{
    class HelpScreen : GameScreen
    {
        bool loadingIsSlow;

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        public Texture2D[] helpImages;
        public Texture2D currentImage;
        public int currentItem;
        public bool isInitial;
        public int seconds;
       private HelpScreen(ScreenManager screenManager, bool loadingIsSlow)
        {
            this.loadingIsSlow = loadingIsSlow;
            helpImages = new Texture2D[4];
            isInitial = true;
            seconds = 0;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TouchPanel.EnabledGestures =
      GestureType.Tap |
      GestureType.DoubleTap |
      GestureType.FreeDrag;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.SpriteBatch.Begin();
           if(isInitial){
              for (int i = 0; i < helpImages.Length; i++)
             {
                 String value = "sprites/helpImage_" + i;
                 helpImages[i] = ScreenManager.Game.Content.Load<Texture2D>(value);
              }
            currentImage = helpImages[0];
            currentItem = 0;
            isInitial = false;
            }
                ScreenManager.SpriteBatch.Draw(currentImage, new Rectangle(0, 0, 800, 450), Color.White);
        
            ScreenManager.SpriteBatch.End();
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            TouchCollection touchCollection = TouchPanel.GetState();
            
            foreach (TouchLocation tl in touchCollection)
            {
                
                if (TouchPanel.IsGestureAvailable &&TouchPanel.ReadGesture().GestureType == GestureType.Tap)
                {
                    if (tl.Position.X > TouchPanel.DisplayWidth/2)
                    {
                        if (currentItem >= helpImages.Length - 1)
                        {
                            //GO TO MAIN MENU!
                            return;
                        }
                        //LOGIC
                        currentItem++;
                        currentImage = helpImages[currentItem];
                        seconds = gameTime.ElapsedGameTime.Seconds;
                    }
                    else
                    {
                        //LOGIC
                        if (currentItem == 0)
                            break;
                        currentItem--;
                        currentImage = helpImages[currentItem];
                        seconds = gameTime.ElapsedGameTime.Seconds;
                    }
                }
            }
           
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                             PlayerIndex? controllingPlayer)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            HelpScreen helpScreen = new HelpScreen(screenManager,
                                                            loadingIsSlow);
          

            screenManager.AddScreen(helpScreen, controllingPlayer);
        }
    }
}

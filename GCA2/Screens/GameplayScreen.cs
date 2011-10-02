#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System.Collections.Generic;
#endregion

namespace GCA2
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        Vector2 touchPosition = new Vector2(0, 0);

        //textures
        Texture2D touchTexture;

        //sprites
        Texture2D happyCloud1;

        //tiles
        Texture2D tileGrass;

        // bg
        Texture2D background;

        // parallax mngr
        ParallaxManager parallaxManager;

        SpriteManager spriteManager;

        WorldObject world;

        Boolean hasStarted = false;

        Boolean isPressed = false;
        int pressedLastX = 0;
        int pressedLastY = 0;

        Random random = new Random();

        float pauseAlpha;

        InputAction pauseAction;
        int newLineY;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                //gameFont = content.Load<SpriteFont>("gamefont");
                //touchTexture = content.Load<Texture2D>("sprites/touch");
                //tileGrass = content.Load<Texture2D>("tiles/WorldLineTexture");
                //happyCloud1 = content.Load<Texture2D>("sprites/happyCloud1");
                //background = content.Load<Texture2D>("bg/background");

                world = new WorldObject(ScreenManager.Game, ScreenManager.SpriteBatch);

                // create worldline queue
                for (int i = 0; i < 800; i++)
                {
                    world.addLine(-1);
                }

                // initiliaze parallax vars
                InitParallex();

                // A real game would probably have more content than this sample, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.
                Thread.Sleep(1000);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

#if WINDOWS_PHONE
            if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
            {
                // playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
                // enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
            }
#endif
        }

        private void InitParallex()
        {
            spriteManager = new SpriteManager(ScreenManager.Game, touchPosition, isPressed, world);

            parallaxManager = new ParallaxManager(ScreenManager.Game);
            parallaxManager.DrawOrder = 0;

            // repeating clouds
            Texture2D layerTexture = content.Load<Texture2D>("bg/parallax_layer");
            Rectangle  rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            parallaxManager.AddLayer(0, 150.0f, true, layerTexture, new Vector2(layerTexture.Width, 10), rect);

            //layerTexture = content.Load<Texture2D>("clouds/clud1");
            //rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            //parallaxManager.AddLayer(0, 90.0f, true, layerTexture, new Vector2(layerTexture.Width, 10), rect);

            //layerTexture = content.Load<Texture2D>("clouds/clud2");
            //rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            //parallaxManager.AddLayer(0, 100.0f, true, layerTexture, new Vector2(layerTexture.Width, 15), rect);

            //layerTexture = content.Load<Texture2D>("clouds/clud3");
            //rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            //parallaxManager.AddLayer(0, 100.0f, true, layerTexture, new Vector2(layerTexture.Width, 15), rect);

            //layerTexture = content.Load<Texture2D>("clouds/clud4");
            //rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            //parallaxManager.AddLayer(0, 100.0f, true, layerTexture, new Vector2(layerTexture.Width, 15), rect);

            layerTexture = content.Load<Texture2D>("bg/background_gradient");
            rect = new Rectangle(0, 0, layerTexture.Width, layerTexture.Height);
            parallaxManager.AddLayer(0, 0.0f, true, layerTexture, new Vector2(layerTexture.Width, 0), rect);





            ScreenManager.Game.Components.Add(parallaxManager);
            ScreenManager.Game.Components.Add(spriteManager);
        }

        public override void Deactivate()
        {
#if WINDOWS_PHONE
            // Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"] = playerPosition;
            // Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"] = enemyPosition;
#endif

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();

#if WINDOWS_PHONE
            //Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("PlayerPosition");
            //Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("EnemyPosition");
#endif
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                #region GAME LOGIC

                for (int i = 0; i < gameTime.ElapsedGameTime.Milliseconds; i++)
                {
                    TouchCollection touchCollection = TouchPanel.GetState();

                    world.removeLine(0);
                    pressedLastX--;

                    if (touchCollection.Count == 0)
                    {
                        isPressed = false;
                        world.addLine(-1);
                    }
                    else
                    {
                        foreach (TouchLocation tl in touchCollection)
                        {
                            if (!hasStarted)
                            {
                                world.getPlayer().setActive();
                                world.fillBegin((int)tl.Position.X, TouchPanel.DisplayHeight - (int)tl.Position.Y);
                                hasStarted = true;
                            }

                            touchPosition.X = tl.Position.X;
                            touchPosition.Y = tl.Position.Y;

                            if (tl.State == TouchLocationState.Pressed || (tl.State == TouchLocationState.Moved))
                            {
                                if (world.getLineQueue()[(int)Math.Max(1, pressedLastX) - 1].Height == -1)
                                {
                                    pressedLastY = (int)touchPosition.Y;
                                }
                                if (touchPosition.X > pressedLastX)
                                {
                                    for (; pressedLastX < touchPosition.X; pressedLastX++)
                                    {
                                        if (pressedLastY == 0)
                                        {
                                            pressedLastY = (int)touchPosition.Y;
                                        }

                                        newLineY = pressedLastY;

                                        if (isPressed)
                                        {
                                            if (pressedLastY < touchPosition.Y)
                                            {
                                                // Decreasing
                                                newLineY = (int)touchPosition.Y;
                                            }
                                            else if (pressedLastY > touchPosition.Y)
                                            {
                                                // Increasing
                                                newLineY -= 2;
                                            }
                                            else
                                            {
                                                // No change
                                                // Do nothing
                                            }

                                            world.addLine(pressedLastX, TouchPanel.DisplayHeight - newLineY);
                                            pressedLastY = newLineY;
                                        }
                                    }
                                    /*
                                    lineQueue.Insert((int)touchPosition.X, new WorldLine(480 - (int)touchPosition.Y));
                                    if (isPressed) // is touch being held?
                                    {
                                        int diffx = Math.Abs(pressedLastX - (int)touchPosition.X);
                                        int diffy = Math.Abs(pressedLastY - (int)touchPosition.Y);
                                        int step = 0;
                                        if (diffx > 0 & diffy > 0)
                                        {
                                            step = Math.Max(diffx, diffy) / Math.Min(diffx, diffy);

                                            for (int i = pressedLastX - 1; i < (int)touchPosition.X; i++)
                                            {
                                                lineQueue.RemoveAt(i);
                                                if (diffx > diffy)
                                                {
                                                    for (int j = 0; j < step; j++)
                                                    {
                                                        lineQueue.Insert(i, new WorldLine(480 - pressedLastY + j));
                                                        i += j;
                                                    }
                                                }
                                                else
                                                {
                                                    lineQueue.Insert(i, new WorldLine(480 - pressedLastY + step));
                                                }
                                            }
                                        }
                                    }
                                    pressedLastX = (int)touchPosition.X;
                                    pressedLastY = (int)touchPosition.Y;
                                    */
                                }
                                else
                                {
                                    world.addLine(-1);
                                }
                                isPressed = true;
                            }

                            break;
                        }
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
#if WINDOWS_PHONE
                ScreenManager.AddScreen(new PhonePauseScreen(), ControllingPlayer);
#else
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
#endif
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (input.TouchState.Count > 0)
                {
                    Vector2 touchPosition = input.TouchState[0].Position;
                    //Vector2 direction = touchPosition - playerPosition;
                    //direction.Normalize();
                    //movement += direction;
                }

                if (movement.Length() > 1)
                    movement.Normalize();

                //playerPosition += movement * 8f;
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}

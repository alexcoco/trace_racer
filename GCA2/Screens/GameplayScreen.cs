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
        Vector2 touchPosition = new Vector2(0, 0);

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
        ParticleEngine part;

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

                List<Texture2D> textures = new List<Texture2D>();
                textures.Add(content.Load<Texture2D>("tex/star"));
                part = new ParticleEngine(textures, new Vector2(400, 240));

                world = new WorldObject(ScreenManager.Game, ScreenManager.SpriteBatch);

                // create worldline queue
                for (int i = 0; i < 800; i++)
                {
                    world.addLine(-1);
                }

                spriteManager = new SpriteManager(ScreenManager.Game, touchPosition, isPressed, world, part);
                ScreenManager.Game.Components.Add(spriteManager);
                Thread.Sleep(1000);

                ScreenManager.Game.ResetElapsedTime();
            }

#if WINDOWS_PHONE
            if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
            {
                // playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
                // enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
            }
        }
#endif
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
                if (!hasStarted || world.player.IsAlive & !world.player.gameOver)
                {
                    #region GAME LOGIC

                    for (int i = 0; i < gameTime.ElapsedGameTime.Milliseconds; i++)
                    {

                        TouchCollection touchCollection = TouchPanel.GetState();

                        world.removeLine(0);
                        pressedLastX--;

                        // create gates
                        if (world.player.Score.Points > 0 & world.player.Score.Points % 5000 == 0)
                        {
                            world.createGates();
                        }

                        //update gates
                        world.updateGates();

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

                                // Detect player-gate collision
                                Gate nextGate = world.getClosestGate(world.player);// Get gates closest intersecting gate

                                if (nextGate != null)    // there is a next gate
                                {
                                    if (!nextGate.isHit)  // gate not yet hit
                                    {
                                        // Add score * ++mutiplier
                                        world.player.Score.Points += 10000 * (++world.player.Score.Multiplier);
                                        // Set gate off
                                        nextGate.gate = nextGate.gatePassed;
                                        nextGate.isHit = true;
                                    }
                                }
                                else
                                {
                                    // Check multiplier and reset if needed
                                    if (world.player.Score.Multiplier > 0)
                                    {
                                        world.player.Score.Multiplier = 0;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //gameover
                    TouchCollection touchCollection = TouchPanel.GetState();
                    foreach (TouchLocation tl in touchCollection)
                    {
                        if (tl.State == TouchLocationState.Pressed || (tl.State == TouchLocationState.Moved))
                        {
                            //ScreenManager.Game.Components.Remove(spriteManager);
                            //ScreenManager.Game.Components.Remove(parallaxManager);
                            //LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new PhoneMenuScreen("Title"));
                            //System.Diagnostics.Debug.WriteLine("SdfdsF");
                            //LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
                        }
                    }
                }
                // particles
                part.EmitterLocation = new Vector2(world.player.Position.X - 35, world.player.Position.Y + 75);
                part.Update();
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
            if (world.player.gameOver)
            {
                // If the game is transitioning on or off, fade it out to black.
                if (TransitionPosition > 0 || pauseAlpha > 0)
                {
                    float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
#if WINDOWS_PHONE
                    ScreenManager.AddScreen(new PhonePauseScreen(), ControllingPlayer);
#else
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
#endif
                    ScreenManager.FadeBackBufferToBlack(alpha);
                }
            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
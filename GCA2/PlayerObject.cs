using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the player game component.
    /// </summary>
    public class PlayerObject : DrawableGameComponent
    {
        public bool IsAlive { get; set; }
        public Texture2D Texture { get; set; }
        //public Rectangle Rectangle { get; set; } // we could use Texture.Bounds instead
        public Vector2 Position;
        public Vector2 Speed; // Will be used for positioning
        public WorldObject World { get; set; }
        public WorldLine CurrentLine { get; set; }
        public Score Score { get; set; }
        Boolean isActive = false;
        public int airtime = 0;

        private SpriteBatch mySpriteBatch;

        /// <summary>
        /// Constructor initializes everything.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="world"></param>
        public PlayerObject(Game game, SpriteBatch givenSpriteBatch, WorldObject world)
            : base(game)
        {
            World = world;
            mySpriteBatch = givenSpriteBatch;

            IsAlive = false;
            Texture = Game.Content.Load<Texture2D>("bike");

            Speed = new Vector2(Constants.NORMAL_SPEED, 0);
            //Position = new Vector2(Speed.X, -Texture.Height);
            Position = new Vector2(100, -Texture.Height);
            CurrentLine = world.getLine((int)Position.X);
        }

        public void setActive()
        {
            isActive = true;
        }

        public void setPosition(Vector2 v)
        {
            Position.X = v.X;
            Position.Y = v.Y;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count != 0)
            {
                IsAlive = true;
            }

            if (isActive)
            {
                //Position.X++;
                int difference = World.getPositionDifference();
                adjustSpeed(difference);
                // Check if touching the world
                if (difference == 0)
                {
                    // Touching world
                    airtime = 0;
                }
                else if (difference < 0)
                {
                    airtime++;
                    // In the air

                    float dropSize = Math.Abs(Position.Y + Texture.Height - (TouchPanel.DisplayHeight - World.getLine((int)Position.X).Height));
                    int yDiff = 3;

                    if (dropSize > 45)
                    {
                        yDiff = (airtime / (6 * gameTime.ElapsedGameTime.Milliseconds));
                    }

                    Position.Y += yDiff;

                    int diff2 = World.getPositionDifference();

                    while (diff2 > 0)
                    {
                       Position.Y -= 1;
                        diff2 = World.getPositionDifference();
                    }
                }
                else // difference > 0
                {
                    // Collision
                    airtime = 0;

                    if (difference > TouchPanel.DisplayHeight*0.1)
                    {
                        IsAlive = false;
                        Position.Y = TouchPanel.DisplayHeight;
                    }

                    do
                    {
                        Position.Y--;
                    } while (World.getPositionDifference() > 0);
                }
                if ((Position.Y + Texture.Height) > TouchPanel.DisplayHeight)
                {
                    IsAlive = false;
                }
                base.Update(gameTime);
            }
        }

        private void adjustSpeed(int difference)
        {
            int delta = CurrentLine.Height - World.getLine((int)(Position.X + Texture.Width / 2) - 1).Height;
            if (difference > 0)
            {
                // decel - bring to min
                if (Position.X > Constants.MIN_SPEED)
                {
                    Position.X--;
                }
            }
            else if (difference < 0)
            {
                // accel - bring to max
                if (Position.X < Constants.MAX_SPEED)
                {
                    Position.X++;
                }
            }
            else
            {
                // flat - bring to normal speed
                if(Position.X > Constants.NORMAL_SPEED){
                    Position.X--;
                }else if (Position.X < Constants.NORMAL_SPEED) {
                    Position.X++;
                }
            }
            
            //if (difference == 0 || difference < 0)
            //{
            //    //// flat || in air
            //    if (Position.X > Constants.NORMAL_SPEED)
            //    {
            //        Position.X--;
            //    }
            //    else if (Position.X < Constants.NORMAL_SPEED)
            //    {
            //        Position.X++;
            //    }
            //} 

        }

        /// <summary>
        /// Draws the player on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}

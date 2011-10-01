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
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public WorldObject World { get; set; }
        public WorldLine CurrentLine { get; set; }
        public Score Score { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="world"></param>
        public PlayerObject(Game game, WorldObject world)
            : base(game)
        {
            World = world;
            IsAlive = true;
            Position = Vector2.Zero;
            Speed = Vector2.Zero;
            CurrentLine = world.getLine((int )Position.X);
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
            // TODO: Add your update code here

            base.Update(gameTime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (this.IsAlive)
            {
                //mySpriteBatch.Draw(texture, position, Color.White);
            }

            base.Draw(gameTime);
        }
    }
}

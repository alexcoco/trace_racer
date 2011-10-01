using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCA2
{
    /// <summary>
    /// Keeps track of the world that the user builds.
    /// </summary>
    public class WorldObject
    {
        Game game;
        SpriteBatch spriteBatch;

        List<WorldLine> lineQueue = new List<WorldLine>(800);

        public PlayerObject player;

        public WorldObject(Game g, SpriteBatch sb)
        {
            game = g;
            spriteBatch = sb;
            player = new PlayerObject(game, sb, this);
            game.Components.Add(player);
        }

        public PlayerObject getPlayer()
        {
            return player;
        }

        public List<WorldLine> getLineQueue()
        {
            return lineQueue;
        }

        /// <summary>
        /// Adds a specific line to the end of the world.
        /// </summary>
        public void addLine(int height)
        {
            lineQueue.Add(new WorldLine(height));
        }

        /// <summary>
        /// Adds a specific line to the specified position of the  world.
        /// </summary>
        public void addLine(int pos, int height)
        {
            lineQueue.Insert(pos, new WorldLine(height));
        }

        public void removeLine(int pos)
        {
            lineQueue.RemoveAt(pos);
        }

        /// <summary>
        /// Gets a specific line from the world.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <returns></returns>
        public WorldLine getLine(int lineNum)
        {
            // Do bounds check first

            return new WorldLine(10);
        }

        /// <summary>
        /// Collision detection between world and player.
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        public bool IsColliding(PlayerObject playerObject)
        {
            // TODO: Add collision detection between world and player
            return false;
        }

        /// <summary>
        /// Checks if player touches the top of the world.
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        internal bool IsTouching(PlayerObject playerObject)
        {
            if (playerObject.Position.Y > lineQueue[(int)playerObject.Position.X + playerObject.Texture.Width / 2].Height)
            {
                return false;
            }
 
            return true;
        }
    }
}

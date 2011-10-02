using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace TraceRacer
{
    /// <summary>
    /// Keeps track of the world that the user builds.
    /// </summary>
    public class WorldObject : DrawableGameComponent
    {
        static Random ran = new Random();
        Game game;
        SpriteBatch spriteBatch;

        List<WorldLine> lineQueue = new List<WorldLine>(800);
        List<Gate> gateQueue = new List<Gate>(14);

        public PlayerObject player;

        public WorldObject(Game g, SpriteBatch sb)
            : base(g)
        {
            game = g;
            spriteBatch = sb;
            player = new PlayerObject(game, sb, this);
            game.Components.Add(player);
        }

        public List<Gate> getGates()
        {
            return gateQueue;
        }

        public void createGates()
        {
            gateQueue.Clear();
            int yval = ran.Next(0, TouchPanel.DisplayHeight-107);

            int randomness = ran.Next(10, getGates().Capacity+1);

            //int offset = ran.Next(-1, 2);

            for (int j = 0; j < randomness; j++)
            {
                // TODO MAYBE CHANGE 200 TO VARYING X OVER TIME
                gateQueue.Add(new Gate(game, spriteBatch, 300 * j, 200 + (int)(ran.NextDouble() * 150)));
            }
        }

        public void updateGates()
        {
            for (int i = 0; i < getGates().Count; i++)
            {
                gateQueue[i].position.X--;
            }
        }

        /// <summary>
        /// Gets the closest gate that intersects with the player.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Gate getClosestGate(PlayerObject player)
        {
            Gate closestGate = null;

            float gX, gX2, gY, gY2; // Gate Position X, Y - should be 10px less on Y due to perspective
            float pX, pX2, pY, pY2; // Player Position X,y
            pX = player.Position.X;
            pX2 = player.Position.X + player.Texture.Width;
            pY = player.Position.Y;
            pY2 = player.Position.Y + player.Texture.Height;
            
            foreach(Gate gate in gateQueue)
            {
                gX = gate.position.X;
                gX2 = gX + gate.myTexture.Width;
                gY = gate.position.Y +20;
                gY2 = gY + gate.myTexture.Height -35;

                if (!gate.isHit && 
                    ((gX <= pX) && (pX <= gX2) ||
                    (gX <= pX2) && (pX2 <= gX2))) // player and gate intersect on X
                {
                    if ((gY <= pY) && (pY <= gY2) ||
                        (gY <= pY2) && (pY2 <= gY2)) // player and gate intersect on Y
                    {
                        closestGate = gate;
                        break;
                    }
                }
                else if (gX > pX2)
                {
                    // This gate is still far, don't look for next ones in this cycle
                    break;
                }
            }

            return closestGate;
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
            if (pos >= 0)
            lineQueue.Insert(pos, new WorldLine(height));
        }

        public void fillBegin(int pos, int height)
        {
            for (int i = 0; i < pos; i++)
            {
                lineQueue.Insert(i, new WorldLine(height));
            }
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
            if (lineNum >= lineQueue.Count)
            {
                return new WorldLine(10);
            }
            return lineQueue[lineNum];
        }

        /// <summary>
        /// Collision detection between world and player.
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        public bool IsColliding(PlayerObject playerObject)
        {
            int value = (int)playerObject.Position.Y + playerObject.Texture.Height;
            value += lineQueue[(int)playerObject.Position.X + playerObject.Texture.Width / 2].Height;
            if (value > TouchPanel.DisplayHeight)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if player touches the top of the world.
        /// </summary>
        /// <param name="playerObject"></param>
        /// <returns></returns>
        internal bool IsTouching(PlayerObject playerObject)
        {
            int value = (int)playerObject.Position.Y + playerObject.Texture.Height;
            value += lineQueue[(int)playerObject.Position.X + playerObject.Texture.Width / 2].Height;
            if (value == TouchPanel.DisplayHeight)
            {
                return true;
            }

            return false;
        }

        public int getPositionDifference()
        {
            int value = (int)player.Position.Y + player.Texture.Height;
            value += lineQueue[(int)player.Position.X + player.Texture.Width / 2].Height;
            return value - TouchPanel.DisplayHeight;
        }
    }
}

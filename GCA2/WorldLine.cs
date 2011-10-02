using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GCA2
{
    /// <summary>
    /// A line is the most basic component of the world.
    /// </summary>
    public class WorldLine
    {
        public int Height { get; set; }

        // static Random r = new Random();
        // public int texture;

        public WorldLine()
        {
            Height = 0;
        }

        public WorldLine(int h)
        {
            Height = h;
            // texture = r.Next(1000);
        }
    }
}

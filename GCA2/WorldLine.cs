using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCA2
{
    /// <summary>
    /// A line is the most basic component of the world.
    /// </summary>
    public class WorldLine
    {
        public int Height { get; set; }

        public WorldLine()
        {
            Height = 0;
        }

        public WorldLine(int h)
        {
            Height = h;
        }

    }
}

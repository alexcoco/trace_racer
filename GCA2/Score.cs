using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GCA2
{
    /// <summary>
    /// Keeps track of the number of points that the player scored.
    /// </summary>
    public class Score : DrawableGameComponent
    {
        public long Points { get; set; }
        public int Multiplier { get; set; }

        public Score(Game game) : base(game)
        {
            Points = 0;
            Multiplier = 0;
        }
    }
}

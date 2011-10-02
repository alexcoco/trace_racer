using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TraceRacer
{
    /// <summary>
    /// Keeps track of the number of points that the player scored.
    /// </summary>
    public class Score : DrawableGameComponent
    {
        public long Points { get; set; }
        public int Multiplier { get; set; }
        //public long Highscore { get; set; }

        public Score(Game game) : base(game)
        {
            Points = 0;
            Multiplier = 0;
            //Highscore = TraceRacer.Highscore.Load();
        }
    }
}

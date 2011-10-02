using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GCA2
{
    public static class Constants
    {
        public const float SPEED = 400.0f; // Represents actual pixel distance from the left of the screen

        public const float MIN_SPEED = SPEED * 0.1f;
        public const float NORMAL_SPEED = SPEED * 0.4f;
        public const float MAX_SPEED = SPEED * 0.8f;

        public const float ROTATION_ANGLE_UP = -0.5f % MathHelper.Pi;
        public const float ROTATION_ANGLE_NORMAL = 0f;
        public const float ROTATION_ANGLE_DOWN = 0.25f % MathHelper.Pi;
    }
}

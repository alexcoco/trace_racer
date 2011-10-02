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

    public class Gate : DrawableGameComponent
    {
        public Boolean isHit;
        public Game game;
        public SpriteBatch spriteBatch;

        public Texture2D myTexture;

        static public Texture2D blueGate;
        static public Texture2D greenGate;
        static public Texture2D yellowGate;
        static public Texture2D indigoGate;
        static public Texture2D orangeGate;
        static public Texture2D pinkGate;
        static public Texture2D redGate;
        static public Texture2D violetGate;
        static public Texture2D gatePassed;

        public Vector2 position;
        static Random rand = new Random();

        public Gate(Game g, SpriteBatch sb, int x, int y)
            : base(g)
        {
            blueGate = g.Content.Load<Texture2D>("gates/BlueGate");
            greenGate = g.Content.Load<Texture2D>("gates/GreenGate");
            indigoGate = g.Content.Load<Texture2D>("gates/IndigoGate");
            orangeGate = g.Content.Load<Texture2D>("gates/OrangeGate");
            pinkGate = g.Content.Load<Texture2D>("gates/PinkGate");
            redGate = g.Content.Load<Texture2D>("gates/RedGate");
            violetGate = g.Content.Load<Texture2D>("gates/VioletGate");
            yellowGate = g.Content.Load<Texture2D>("gates/YellowGate");
            gatePassed = g.Content.Load<Texture2D>("gates/GrayGate");

            myTexture = genTexture();
            isHit = false;
            game = g;
            spriteBatch = sb;

            position = new Vector2(2000 + x, y);
        }
        public override void Update(GameTime gameTime)
        {
            //position.X--;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        public Texture2D genTexture()
        {
            switch (rand.Next(0, 7))
            {
                case 0:
                    return blueGate;
                case 1:
                    return greenGate;
                case 2:
                    return yellowGate;
                case 3:
                    return indigoGate;
                case 4:
                    return orangeGate;
                case 5:
                    return pinkGate;
                case 6:
                    return violetGate;
                default: return blueGate;
            }
        }
    }
}

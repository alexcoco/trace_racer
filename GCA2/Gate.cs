using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GCA2
{

    public class Gate : DrawableGameComponent
    {
        public Boolean isHit;
        public Game game;
        public SpriteBatch spriteBatch;
        public Texture2D gate;
        public Texture2D gatePassed;
        public Vector2 position;
        public int numberOfMillies;
        static Random rand = new Random();
        public Gate(Game g, SpriteBatch sb, int x)
            : base(g)
        {
            isHit = false;
            game = g;
            spriteBatch = sb;
            gate = game.Content.Load<Texture2D>("gates/gate0");
            gatePassed = game.Content.Load<Texture2D>("gates/gate1");
            numberOfMillies = 800;
            int value = rand.Next(0, TouchPanel.DisplayHeight/2);
            position = new Vector2(1000 + x, value);
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
    }
}

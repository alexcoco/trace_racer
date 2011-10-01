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
        public Game game;
        public SpriteBatch spriteBatch;
        public Texture2D gate;
        public Vector2 position;
        public int numberOfMillies;
        static Random rand = new Random();
        public Gate(Game g, SpriteBatch sb)
            : base(g)
        {
            game = g;
            spriteBatch = sb;
            gate = game.Content.Load<Texture2D>("sprites/gates/gate");
            numberOfMillies = 800;
            int value = rand.Next(0, TouchPanel.DisplayHeight/2);
            position = new Vector2(800+ numberOfMillies, value );
        }
        public override void Update(GameTime gameTime)
        {
            position.X--;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(gate,new Rectangle((int)position.X, (int)position.Y, gate.Width, gate.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

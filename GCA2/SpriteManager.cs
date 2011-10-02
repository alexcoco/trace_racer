using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace GCA2
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;

        ContentManager content;

        Vector2 touchPosition = new Vector2(0, 0);
        Vector2 parallaxPosition = new Vector2(0, 40);
        Vector2 parallaxSpeed = new Vector2(0.5f, 0);
        Texture2D parallax;
        Texture2D touchTexture;
        Texture2D tileGrass;
        Texture2D gameOverlay;
        Texture2D background;
        Texture2D tree2;
        Texture2D tree3;
        Texture2D tree4;
        Texture2D gatekeeper;
        SpriteFont mainFont;
        WorldObject world;
        Boolean isPressed = false;
        ParticleEngine part;

        public SpriteManager(Game game, Vector2 touchPosition, Boolean isPressed, WorldObject world, ParticleEngine part)
            : base(game)
        {
            this.touchPosition = touchPosition;
            this.isPressed = isPressed;
            this.world = world;

            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.part = part;

            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            mainFont = content.Load<SpriteFont>("mainFont");
            touchTexture = content.Load<Texture2D>("sprites/touch");
            tileGrass = content.Load<Texture2D>("tiles/WorldLineTexture");
            gameOverlay = content.Load<Texture2D>("bg/gameOver");
            background = content.Load<Texture2D>("bg/background_gradient");
            parallax = content.Load<Texture2D>("bg/parallax_layer_cont");
            gatekeeper = content.Load<Texture2D>("gates/gatekeeper");

            tree2 = content.Load<Texture2D>("sprites/tree2");
            tree3 = content.Load<Texture2D>("sprites/tree3");
            tree4 = content.Load<Texture2D>("sprites/tree4");
        }

        public override void Update(GameTime gameTime)
        {
            if (parallaxPosition.X > -800f)
            {
                parallaxPosition -= parallaxSpeed;
            }
            else
            {
                // Reset the position
                parallaxPosition.X = 0;
            }

            base.Update(gameTime);
        }

        public void ParticleTime()
        {
            part.Draw(spriteBatch);
        }


        public override void Draw(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(parallax, parallaxPosition, Color.White);

            if (!world.player.gameOver)
            {
                //if (isPressed)
                   //spriteBatch.Draw(touchTexture, new Vector2(touchPosition.X - 40, touchPosition.Y - 40), Color.White);

                for (int i = 0; i < world.getLineQueue().Count; i++)
                {
                    spriteBatch.Draw(tileGrass, new Vector2(i, 480 - world.getLineQueue()[i].Height), Color.White);
                }

                if (world.player.IsAlive)
                {
                    drawPlayer(world);
                }

                if (!(world.getGates().Count == 0) && world.getGates()[0].position.X > 800 && world.getGates()[0].position.X < 1600) 
                {
                    spriteBatch.Draw(gatekeeper, new Vector2(705, world.getGates()[0].position.Y), Color.White);
                }

                for (int j = 0; j < world.getGates().Count; j++)
                {
                    spriteBatch.Draw(world.getGates()[j].myTexture, new Vector2(world.getGates()[j].position.X, world.getGates()[j].position.Y), Color.White);
                }

                // Display score - lose one precision digit and mask the second one
                spriteBatch.DrawString(mainFont, "SCORE: " + (world.player.Score.Points / 100 * 10).ToString(), new Vector2(5, -10), Color.White);
            }
            else
            {
                spriteBatch.Draw(gameOverlay, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(mainFont, "SCORE: " + world.player.Score.Points, new Vector2(200 - mainFont.Spacing, 260), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawPlayer(WorldObject world)
        {
            float rotationAngle;
            Vector2 origin;

            //spriteBatch.Draw(Texture, Position, Color.White);

            int difference = world.getPositionDifference();
            if (difference > 0)
            {
                // decel - bring to min
                rotationAngle = Constants.ROTATION_ANGLE_DOWN;
                origin = new Vector2(0, 0);
            }
            else if (world.player.airtime > 14)
            {
                // accel - bring to max
                rotationAngle = Constants.ROTATION_ANGLE_UP;
                origin = new Vector2(100, 10);
            }
            else
            {
                // flat - bring to normal speed
                rotationAngle = Constants.ROTATION_ANGLE_NORMAL;
                origin = new Vector2(40, -10);
            }

            spriteBatch.Draw(world.player.Texture, world.player.Position, null, Color.White, rotationAngle, origin, 1f, SpriteEffects.None, 0);
        }
    }
}

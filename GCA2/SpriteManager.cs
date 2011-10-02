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
        Texture2D touchTexture;
        Texture2D tileGrass;
        Texture2D gameOverlay;
        SpriteFont mainFont;
        WorldObject world;
        Boolean isPressed = false;

        public SpriteManager(Game game, Vector2 touchPosition, Boolean isPressed, WorldObject world)
            : base(game)
        {
            this.touchPosition = touchPosition;
            this.isPressed = isPressed;
            this.world = world;

            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);

            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            mainFont = content.Load<SpriteFont>("mainFont");
            touchTexture = content.Load<Texture2D>("sprites/touch");
            tileGrass = content.Load<Texture2D>("tiles/WorldLineTexture");
            gameOverlay = content.Load<Texture2D>("bg/gameOver");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();

            if (!world.player.gameOver)
            {
                if (isPressed)
                    spriteBatch.Draw(touchTexture, new Vector2(touchPosition.X - 40, touchPosition.Y - 40), Color.White);

                for (int i = 0; i < world.getLineQueue().Count; i++)
                {
                    spriteBatch.Draw(tileGrass, new Vector2(i, 480 - world.getLineQueue()[i].Height), Color.White);
                }

                if (world.player.IsAlive)
                {
                    drawPlayer(world);
                }

                for (int j = 0; j < world.getGates().Count; j++)
                {
                    spriteBatch.Draw(world.getGates()[j].gate, new Vector2(world.getGates()[j].position.X, world.getGates()[j].position.Y), Color.White);
                }

                // Display score - lose one precision digit and mask the second one
                spriteBatch.DrawString(mainFont, "SCORE: " + (world.player.Score.Points / 100 * 10).ToString(), new Vector2(15, 10), Color.White);
            }
            else
            {
                spriteBatch.Draw(gameOverlay, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(mainFont, "SCORE: " + world.player.Score.Points, new Vector2(200 - mainFont.Spacing, 60), Color.White);
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

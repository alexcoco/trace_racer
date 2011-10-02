using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GCA2
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;

        ContentManager content;

        Vector2 touchPosition = new Vector2(0, 0);

        //textures
        Texture2D touchTexture;

        //sprites
        Texture2D happyCloud1;

        //tiles
        Texture2D tileGrass;

        // bg
        Texture2D background;

        SpriteFont gameFont;
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

            gameFont = content.Load<SpriteFont>("gamefont");
            touchTexture = content.Load<Texture2D>("sprites/touch");
            tileGrass = content.Load<Texture2D>("tiles/WorldLineTexture");
            happyCloud1 = content.Load<Texture2D>("sprites/happyCloud1");
            background = content.Load<Texture2D>("bg/background");
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Our player and enemy are both actually just text strings.
            //SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            //spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //spriteBatch.Draw(happyCloud1, new Vector2(10, 10), new Rectangle(-1, 0, happyCloud1.Width, happyCloud1.Height), Color.White);


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

            //spriteBatch.DrawString(gameFont, touchPosition.X + " " + touchPosition.Y + " " + newLineY + " " + pressedLastY, new Vector2(0, 0), Color.White);
            //spriteBatch.DrawString(gameFont,
            ////     "Tx: " + touchPosition.X
            //   + "\nTy: " + touchPosition.Y
            //   + "\nNy: " + newLineY
            //   + "\nPx: " + pressedLastX
            //   + "\nPy: " + pressedLastY,
            //   new Vector2(0, 0), Color.White);
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

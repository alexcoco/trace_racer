/*
 * Author: Chad Mowery
 * Date: 11/22/2010
 * 
 * Please refer to the README.txt file for usage.
 * Distributed under the BSD open source license.
 */
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
    /// /// <summary>
    /// The ParallexManager class implements IUpdateable and is responsible 
    /// for the creation and management of internal Layer objects. Each Layer 
    /// object represents a scrolling Z-ordered background element.
    /// </summary>
    public class ParallaxManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private int layerCount;
        private List<Layer> layers;

        /// <summary>
        /// Constructs a new ParallexManager object and initializes
        /// it's internal state.
        /// </summary>
        /// <param name="game"></param>
        public ParallaxManager(Game game)
            : base(game)
        {
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);

            this.layerCount = 0;
            this.layers = new List<Layer>(3);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }


        /// <summary>
        /// Add a new Z-Layer to the ParallexManager object at the passed Z-level.
        /// If the requested Z-Layer is already in use the manager will "push" the
        /// current layer up a Z-level. That is, the old layer will be displayed over
        /// top of the newly added layer.
        /// </summary>
        /// <param name="zOrder">The requested Z-level of the new layer.</param>
        /// <param name="scrollSpeed">The scroll speed of the new layer.</param>
        /// <param name="isSeamless">Specifies whether the layer should be repeated.</param>
        /// <param name="texture">The background texture to use for the new layer.</param>
        /// <param name="position">The position of the new layer.</param>
        /// <param name="sourceRect">The texture source rectangle.</param>
        public void AddLayer(int zOrder, float scrollSpeed, bool isSeamless, Texture2D texture, Vector2 position, Rectangle sourceRect)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(zOrder >= 0);
            System.Diagnostics.Debug.Assert(scrollSpeed >= 0);
            System.Diagnostics.Debug.Assert(texture != null);
            System.Diagnostics.Debug.Assert(position != null);
            System.Diagnostics.Debug.Assert(sourceRect != null);
#endif
            Layer newLayer = new Layer(zOrder, scrollSpeed, isSeamless, texture, position, sourceRect);
            int oldCount = layers.Count;
            layers.Insert(zOrder, newLayer);

            if (layers.Count > oldCount)
            {
                layerCount++;
            }
        }


        /// <summary>
        /// Refreshes a currently bound Z-Layer in the ParallexManager object at the passed Z-level.
        /// If the requested Z-Layer is not already in use the manager will add the
        /// current layer at that Z-level.
        /// </summary>
        /// <param name="zOrder">The requested Z-level of the new layer.</param>
        /// <param name="scrollSpeed">The scroll speed of the new layer.</param>
        /// <param name="isSeamless">Specifies whether the layer should be repeated.</param>
        /// <param name="texture">The background texture to use for the new layer.</param>
        /// <param name="position">The position of the new layer.</param>
        /// <param name="sourceRect">The texture source rectangle.</param>
        public void RefreshLayer(int zOrder, float scrollSpeed, bool isSeamless, Texture2D texture, Vector2 position, Rectangle sourceRect)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(zOrder >= 0);
            System.Diagnostics.Debug.Assert(zOrder <= layers.Count);
            System.Diagnostics.Debug.Assert(scrollSpeed >= 0);
            System.Diagnostics.Debug.Assert(texture != null);
            System.Diagnostics.Debug.Assert(position != null);
            System.Diagnostics.Debug.Assert(sourceRect != null);
#endif
            int oldCount = layers.Count;
            Layer oldLayer = layers[zOrder];

            if (oldLayer != null)
            {
                oldLayer.scrollSpeed = scrollSpeed;
                oldLayer.isSeamless = isSeamless;
                oldLayer.layerTexture = texture;
                oldLayer.position = position;
            }
            else
            {
                Layer newLayer = new Layer(zOrder, scrollSpeed, isSeamless, texture, position, sourceRect);
                layers.Insert(zOrder, newLayer);
            }
            if (layers.Count > oldCount)
            {
                layerCount++;
            }
        }


        /// <summary>
        /// Removes a layer at the specified Z-level. Removed layers are no longer
        /// updated or drawn by the ParallexManager object. If no layer is present
        /// at the requested level then noop.
        /// </summary>
        /// <param name="zOrder">The requested z-Level to remove.</param>
        public void RemoveLayer(int zOrder)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(zOrder >= 0);
#endif
            int oldCount = layers.Count;
            layers.RemoveAt(zOrder);

            if (layers.Count < oldCount)
            {
                layerCount--;
            }
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Layer layer in layers)
            {
                if (layer.isSeamless)
                {
                    layer.position.X -= layer.scrollSpeed * timeDelta;
                    layer.position.X = layer.position.X % layer.layerTexture.Width;
                }
                else
                {
                    if (layer.position.X >= Game.GraphicsDevice.Viewport.Width)
                    {
                        layer.position.X = -layer.layerTexture.Width;
                    }
                    else
                    {
                        layer.position.X += layer.scrollSpeed * timeDelta;
                    }
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();

            foreach (Layer layer in layers)
            {
                // draw the main layer
                spriteBatch.Draw(layer.layerTexture, layer.position, layer.sourceRectangle, Color.White);

                // now draw the trailing/leading layer
                if (layer.isSeamless)
                {
                    Vector2 offset = layer.position - new Vector2(layer.layerTexture.Width, 0);
                    spriteBatch.Draw(layer.layerTexture, offset, layer.sourceRectangle, Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// The Layer Internal class is a simple container for individual scrolling Z-layer
    /// objects. All Layers are owned explicitely by the ParallexManager object.
    /// </summary>
    internal class Layer
    {
        #region Layer Members

        public int zOrder;
        public float scrollSpeed;
        public bool isSeamless;
        public Texture2D layerTexture;
        public Vector2 position;
        public Rectangle sourceRectangle;

        #endregion

        /// <summary>
        /// Constructs a new Layer container.
        /// </summary>
        /// <param name="zOrder">The Z-level of this layer.</param>
        /// <param name="speed">The scroll speed of this layer.</param>
        /// <param name="isSeamless">Specifies whether the layer should be repeated.</param>
        /// <param name="texture">The background texture of this layer.</param>
        /// <param name="position">The position of this layer.</param>
        /// <param name="sourceRect">The texture source rectangle.</param>
        public Layer(int zOrder, float speed, bool isSeamless, Texture2D texture, Vector2 position, Rectangle sourceRect)
        {
            this.zOrder = zOrder;
            this.scrollSpeed = speed;
            this.isSeamless = isSeamless;
            this.layerTexture = texture;
            this.position = position;
            this.sourceRectangle = sourceRect;
        }
        #region Layer Member Funtions
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CurveGame
{
    class Camera : DrawableGameComponent
    {
        Vector3 position = new Vector3(0.0f);
        Vector3 look_at = new Vector3(0.0f);

        Matrix projMatrix = new Matrix();
        public Matrix projection
        {
            get
            {
                return projMatrix;
            }
        }

        Matrix viewMatrix = new Matrix();
        public Matrix view
        {
            get
            {
                return viewMatrix;
            }
        }

        public Camera(Game game) 
            : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Create a matrix to convert from pixel coordinates to clip space
            // So, before transformation, the screen boundaries go from 0 to (say) 640 horizontally,
            // and 0 to (say) 480 vertically.  After transformation, they go from -1 to 1 horizontally,
            // and -1 to 1 vertically.  Note in pixel coordinates, increasing Y values are further down 
            // the screen, while in clip space, increasing Y values are further up the screen -- so there
            // is a Y scaling factor of -1.
            float viewportWidth = (float)GraphicsDevice.Viewport.Width;
            float viewportHeight = (float)GraphicsDevice.Viewport.Height;
            float scaleX = 1.0f / (viewportWidth / 2);
            float scaleY = 1.0f / (viewportHeight / 2);
            projMatrix = Matrix.CreateScale(scaleX, scaleY, 1) *
                         Matrix.CreateScale(1, -1, 1) *
                         Matrix.CreateTranslation(-1, 1, 0);
            viewMatrix = Matrix.CreateTranslation(0.0F, 0.0F, 0.0F) * Matrix.CreateRotationZ(0.0F) * Matrix.CreateScale(1.0f /*/ 10.0F*/, 1.0f /*/ 10.0F*/, 1.0f);
        }
    }
}

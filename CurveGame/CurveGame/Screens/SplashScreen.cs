using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace CurveGame
{
    class SplashScreen : GameScreen
    {
        ContentManager content;
        Texture2D splashTexture;
        String texture = "";

        public SplashScreen(String tex)
        {
            TransitionOnTime = TimeSpan.FromSeconds(3.0f);
            TransitionOffTime = TimeSpan.FromSeconds(3.0f);
            texture = tex;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            splashTexture = content.Load<Texture2D>("Images/" + texture);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(splashTexture, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.End();
        }
    }
}

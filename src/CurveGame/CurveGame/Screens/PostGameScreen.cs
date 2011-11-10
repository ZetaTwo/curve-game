using System;
using System.Collections.Generic;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections;

namespace CurveGame
{
    class PostGameScreen : GameScreen
    {
        class comparePlayers : IComparer<Settings.Player>
        {
            int IComparer<Settings.Player>.Compare(Settings.Player x, Settings.Player y)
            {
                if (x.score > y.score)
                    return -1;
                else if (x.score < y.score)
                    return 1;
                else
                    return 0;
            }

        }

        List<Settings.Player> players = new List<Settings.Player>();

        public PostGameScreen()
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;

            for (int i = 0; i < 6; i++)
            {
                if (Settings.players[i].active)
                    players.Add(Settings.players[i]);
            }

            IComparer<Settings.Player> compareplayers = new comparePlayers();
            players.Sort(compareplayers);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            if (TransitionPosition == 0)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;
                Viewport view = ScreenManager.Game.GraphicsDevice.Viewport;
                float posx = view.Width / 2 - 100;
                float posy = view.Height / 2 - 200;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Results:", new Vector2(posx,posy + 20), Color.White);

                int position = 1;
                foreach(Settings.Player player in players)
                {
                    spriteBatch.DrawString(font, position.ToString() + ". Player " + player.number.ToString() + ": " + player.score.ToString(), new Vector2(posx, posy + 40 * position + 60), player.color);
                    position++;
                }

                spriteBatch.End();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.MenuSelect)
            {
                ScreenManager.AddScreen(new PreGameScreen());
                ExitScreen();
            }
        }
    }
}

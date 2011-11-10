using System;
using System.Collections.Generic;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CurveGame
{
    class PreGameScreen : GameScreen
    {
        public PreGameScreen()
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;

            for (int i = 0; i < 6; i++)
            {
                Settings.players[i].active = false;
                Settings.players[i].score = 0;
                Settings.Game.num_players = 0;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            if (TransitionPosition == 0)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Players:", new Vector2(20, 20), Color.White);
                for (int i = 0; i < 6; i++)
                {
                    spriteBatch.DrawString(font, "Player " + Settings.players[i].number.ToString() + " (" + Settings.players[i].controls[0].ToString() + ", " + Settings.players[i].controls[1].ToString() + "):" + ((Settings.players[i].active) ? " Ready" : ""), new Vector2(20, 40 * i + 60), Settings.players[i].color);
                }

                if (Settings.Game.num_players > 1)
                    spriteBatch.DrawString(font, "Press Space/A button to start game!", new Vector2(20, 350), Color.White);

                //spriteBatch.DrawString(font, "Info\n text\n about\n game w/\n picture\n", new Vector2(400, 100), Color.White);
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

            for (int i = 0; i < 6; i++)
            {
                if (Settings.players[i].active == false && input.IsPlayerTurnLeft((PlayerNumber)i))
                {
                    Settings.players[i].active = true;
                    Settings.Game.num_players++;
                }
                else if (Settings.players[i].active == true && input.IsPlayerTurnRight((PlayerNumber)i))
                {
                    Settings.players[i].active = false;
                    Settings.Game.num_players--;
                }
            }

            if (Settings.Game.num_players > 1 && input.MenuSelect)
            {
                LoadingScreen.Load(ScreenManager, true, new GamePlayScreen());
                ExitScreen();
            }

            if (input.PauseGame)
            {
                const string message = "Are you sure you want to quit this game?";
                MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
                confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
                ScreenManager.AddScreen(confirmQuitMessageBox);
            }
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}

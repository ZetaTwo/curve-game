using System;
using System.Collections.Generic;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CurveGame
{
    class GamePlayScreen : GameScreen
    {
        ContentManager content;
        Texture2D playfieldTexture;

        List<Player> players;
        Random random = new Random();
        public Random Random
        {
            get
            {
                return random;
            }
        }

        Camera camera;
        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        bool gameOver = false;
        bool roundOver = false;

        public GamePlayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(3.0);
            TransitionOffTime = TimeSpan.Zero;

            players = new List<Player>(); 
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            playfieldTexture = content.Load<Texture2D>("Images/blank");

            ScreenManager.Game.Components.Add(camera = new Camera(ScreenManager.Game));

            for (int i = 0; i < 6; i++)
            {
                if (Settings.players[i].active)
                    players.Add(new Player(this, (PlayerNumber)i));
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            ScreenManager.Game.Components.Remove(camera);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (Player player in players)
            {
                player.Draw(gameTime);
            }

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport view = ScreenManager.Game.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            //Draw borders and scoreboard background
            List<Rectangle> boxes = new List<Rectangle>();
            boxes.Add(new Rectangle(view.Width - 120, 0, 120, view.Height));
            boxes.Add(new Rectangle(0, 0, view.Width - 120, 1));
            boxes.Add(new Rectangle(view.Height - 1, 0, view.Width - 120, 1));
            boxes.Add(new Rectangle(0, 0, 1, view.Height));
            foreach (Rectangle box in boxes)
            {
                spriteBatch.Draw(playfieldTexture, box, Color.CornflowerBlue);
            }

            //Draw scores
            spriteBatch.DrawString(font, "Score:", new Vector2(view.Width - 100, 20), Color.Black);
            for (int i = 0; i < 6; i++)
            {
                Settings.Player player = Settings.players[i];

                if(player.active)
                    spriteBatch.DrawString(font, player.score.ToString(), new Vector2(view.Width - 100, 40 * i + 60), player.color);
            }

            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (roundOver && input.MenuSelect)
            {
                ExitScreen();

                if (gameOver)
                {
                    ScreenManager.AddScreen(new PostGameScreen());
                }
                else
                {
                    ScreenManager.AddScreen(new GamePlayScreen());
                }
            }

            foreach(Player player in players)
            {
                if (input.IsPlayerTurnLeft(player.player))
                   player.Turn(TurnDirection.Left);
               else if (input.IsPlayerTurnRight(player.player))
                   player.Turn(TurnDirection.Right);
            }

            if (input.PauseGame)
            {
                // If they pressed pause, bring up the pause menu screen.
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            camera.Update(gameTime);

            if (!otherScreenHasFocus && !roundOver)
            {
                List<Rectangle> all_other_players = new List<Rectangle>();
                List<Player> alive_players = new List<Player>();
                int players_that_died = 0;

                foreach (Player player in players)
                {
                    player.Update(gameTime);

                    all_other_players.Clear();
                    foreach (Player otherPlayer in players)
                    {
                        List<Rectangle> boxes = otherPlayer.Worm.Rectangles;

                        if (!otherPlayer.Equals(player))
                            all_other_players.AddRange(boxes);
                        else if(boxes.Count - 20 > 0)
                            all_other_players.AddRange(boxes.GetRange(0,boxes.Count - 10));
                    }

                    if (player.Alive)
                    {
                        if (player.Worm.Collide(all_other_players))
                        {
                            player.Die();
                            players_that_died++;
                        }
                        else
                        {
                            alive_players.Add(player);
                        }
                    } 
                }

                if (players_that_died > 0)
                {
                    foreach (Player player in alive_players)
                    {
                        player.Score += players_that_died;
                    }
                }

                if (alive_players.Count <= 1) //If round end
                {
                    endRound();
                }
            }
        }

        protected void endRound()
        {
            roundOver = true;

            foreach (Player player in players)
            {
                if (player.Score >= 10 * (players.Count - 1))
                {
                    gameOver = true;
                }
            }
        }
    }
}

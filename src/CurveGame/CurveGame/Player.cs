using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace CurveGame
{
    class Player
    {
        GamePlayScreen screen;

        Worm worm;
        public Worm Worm
        {
            get
            {
                return worm;
            }
        }

        Color color;
        public PlayerNumber player;
        bool alive = true;
        public bool Alive
        {
            get
            {
                return alive;
            }
        }

        public int Score
        {
            get
            {
                return Settings.players[(int)player].score;
            }
            set
            {
                Settings.players[(int)player].score = value;
            }
        }

        public Player(GamePlayScreen scr, PlayerNumber num)
        {
            screen = scr;
            player = num;
            color = Settings.players[(int)player].color;
            worm = new Worm(screen, color);      
        }

        public void Update(GameTime gameTime)
        {
            if (alive)
            {
                Settings.Player player_settings = Settings.players[(int)player];

                /*if (Keyboard.GetState().IsKeyDown(player_settings.controls[(int)PlayerCommands.TurnLeft]))
                    worm.Turn(TurnDirection.Left);
                else if (Keyboard.GetState().IsKeyDown(player_settings.controls[(int)PlayerCommands.TurnRight]))
                    worm.Turn(TurnDirection.Right);*/

                worm.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            worm.Draw(gameTime);
        }

        public void Die()
        {
            alive = false;
        }

        public void Turn(TurnDirection direction)
        {
            if (alive)
                worm.Turn(direction);
        }
    }
}

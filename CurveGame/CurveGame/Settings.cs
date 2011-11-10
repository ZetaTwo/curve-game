using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CurveGame
{
    struct Settings
    {
        static Settings()
        {

            players[0].controls = new Keys[2];
            players[0].controls[(int)PlayerCommands.TurnLeft] = Keys.D1;
            players[0].controls[(int)PlayerCommands.TurnRight] = Keys.Q;
            players[0].color = Color.Red;
            players[0].active = false;
            players[0].score = 0;
            players[0].number = PlayerNumber.One;

            players[1].controls = new Keys[2];
            players[1].controls[(int)PlayerCommands.TurnLeft] = Keys.LeftControl;
            players[1].controls[(int)PlayerCommands.TurnRight] = Keys.LeftAlt;
            players[1].color = Color.Blue;
            players[1].active = false;
            players[1].score = 0;
            players[1].number = PlayerNumber.Two;

            players[2].controls = new Keys[2];
            players[2].controls[(int)PlayerCommands.TurnLeft] = Keys.M;
            players[2].controls[(int)PlayerCommands.TurnRight] = Keys.OemComma;
            players[2].color = Color.Green;
            players[2].active = false;
            players[2].score = 0;
            players[2].number = PlayerNumber.Three;

            players[3].controls = new Keys[2];
            players[3].controls[(int)PlayerCommands.TurnLeft] = Keys.Left;
            players[3].controls[(int)PlayerCommands.TurnRight] = Keys.Down;
            players[3].color = Color.Yellow;
            players[3].active = false;
            players[3].score = 0;
            players[3].number = PlayerNumber.Four;

            players[4].controls = new Keys[2];
            players[4].controls[(int)PlayerCommands.TurnLeft] = Keys.Multiply;
            players[4].controls[(int)PlayerCommands.TurnRight] = Keys.Subtract;
            players[4].color = Color.Pink;
            players[4].active = false;
            players[4].score = 0;
            players[4].number = PlayerNumber.Five;

            players[5].controls = new Keys[2];
            players[5].controls[(int)PlayerCommands.TurnLeft] = Keys.V;
            players[5].controls[(int)PlayerCommands.TurnRight] = Keys.B;
            players[5].color = Color.Cyan;
            players[5].active = false;
            players[5].score = 0;
            players[5].number = PlayerNumber.Six;
        }

        public struct Worm
        {
            /*public static float movespeed = 0.0000005F;
            public static float turnspeed = 0.0000003F;*/

            public static float movespeed = 0.00001F;
            public static float turnspeed = 0.0000003F;

            public static long point_resolution = 50000;

            public static double hole_chance = 0.005;
            public static long hole_time = 2000000;
            public static long spawn_time = 2000;

            public static int worm_width = 4;
        }
        
        public static Player[] players = new Player[6];

        public struct Player
        {
            public Keys[] controls;
            public Color color;
            public bool active;
            public int score;
            public PlayerNumber number;
        }

        public struct Game
        {
            public static int max_score = 10;
            public static int num_players = 0;
        }
    }
}

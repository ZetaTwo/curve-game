using System;
using System.Collections.Generic;
using System.Text;
using CurveGame;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace CurveGame
{
    class Worm
    {
        GamePlayScreen screen;
        SpriteBatch spriteBatch;
        ContentManager content;
        Texture2D dotTexture;
        Color[] dotTextureData;
        Color color;

        TurnDirection turning;
        Vector2 head_direction = new Vector2(0);
        Vector2 head_position;

        List<Vector2> wormPoints = new List<Vector2>();
        List<Rectangle> rectangles = new List<Rectangle>();
        public List<Rectangle> Rectangles
        {
            get
            {
                return rectangles;
            }
        }

        long last_point_time;
        long hole_begin_time = 0;
        bool making_hole = false;

        public Worm(GamePlayScreen scr, Color col)
        {
            color = col;
            screen = scr;
            turning = TurnDirection.None;

            spriteBatch = screen.ScreenManager.SpriteBatch;

            LoadContent();

            Rectangle spawnfield = new Rectangle(100, 100, screen.ScreenManager.Game.GraphicsDevice.Viewport.Width - 120 - 200, screen.ScreenManager.Game.GraphicsDevice.Viewport.Height - 200);

            setDirection(360.0 * screen.Random.NextDouble());
            head_position = new Vector2(spawnfield.Left + spawnfield.Width * (float)screen.Random.NextDouble(),
                spawnfield.Top + spawnfield.Height * (float)screen.Random.NextDouble());


            AddPoint(CurrentVector());
        }

        protected void LoadContent()
        {
            if (content == null)
                content = new ContentManager(screen.ScreenManager.Game.Services, "Content");

            dotTexture = content.Load<Texture2D>("Images/dot");
            dotTextureData = new Color[dotTexture.Width * dotTexture.Height];
            dotTexture.GetData(dotTextureData);
        }

        public void Update(GameTime gameTime)
        {
            if (screen.TransitionPosition == 0)
            {
                //Hole?
                if (!making_hole && screen.Random.NextDouble() < Settings.Worm.hole_chance)
                {
                    making_hole = true;
                    hole_begin_time = gameTime.TotalGameTime.Ticks;
                }
                else if (making_hole && gameTime.TotalGameTime.Ticks > Settings.Worm.hole_time + hole_begin_time)
                {
                    making_hole = false;
                    hole_begin_time = 0;
                }

                //Turning
                if (turning != TurnDirection.None)
                {
                    double V = Math.Atan2(head_direction.Y, head_direction.X);

                    if (turning == TurnDirection.Left)
                        V -= gameTime.ElapsedGameTime.Ticks * Settings.Worm.turnspeed;
                    else if (turning == TurnDirection.Right)
                        V += gameTime.ElapsedGameTime.Ticks * Settings.Worm.turnspeed;

                    setDirection(V);

                    turning = TurnDirection.None;
                }

                //New position
                head_position.X += head_direction.X * Settings.Worm.movespeed * gameTime.ElapsedGameTime.Ticks;
                head_position.Y += head_direction.Y * Settings.Worm.movespeed * gameTime.ElapsedGameTime.Ticks;

                if (!making_hole && Settings.Worm.point_resolution < (gameTime.TotalGameTime.Ticks - last_point_time))
                {
                    AddPoint(CurrentVector());
                    last_point_time = gameTime.TotalGameTime.Ticks;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (screen.TransitionPosition == 0 || (int)(screen.TransitionPosition * 10) % 2 == 0)
            {
                spriteBatch.Begin();

                Rectangle rect = new Rectangle((int)head_position.X, (int)head_position.Y, dotTexture.Width, dotTexture.Height);
                spriteBatch.Draw(dotTexture, rect, color);

                foreach (Vector2 position in wormPoints)
                {
                    rect = new Rectangle((int)position.X, (int)position.Y, dotTexture.Width, dotTexture.Height);
                    spriteBatch.Draw(dotTexture, rect, color);
                }

                spriteBatch.End();
            }
        }

        private void AddPoint(Vector2 pos)
        {
            wormPoints.Add(pos);
            Rectangles.Add(new Rectangle((int)pos.X, (int)pos.Y, dotTexture.Width, dotTexture.Height));
        }

        private Vector2 CurrentVector()
        {
            return new Vector2(head_position.X, head_position.Y);
        }

        private void setDirection(double angle)
        {
            head_direction.X = (float)Math.Cos(angle);
            head_direction.Y = (float)Math.Sin(angle);
        }

        public void Turn(TurnDirection turn)
        {
            turning = turn;
        }

        public bool Collide(List<Rectangle> boxes)
        {
            Viewport view = screen.ScreenManager.Game.GraphicsDevice.Viewport;
            Rectangle play_field = new Rectangle(0, 0, view.Width - 120, view.Height);
            if (head_position.X < play_field.Left || head_position.X > play_field.Right ||
                head_position.Y < play_field.Top || head_position.Y > play_field.Bottom)
                return true;

            Rectangle head = new Rectangle((int)head_position.X, (int)head_position.Y, dotTexture.Width, dotTexture.Height);
            foreach (Rectangle box in boxes)
            {
                if (head.Intersects(box))
                    return IntersectPixels(box, dotTextureData,
                                    head, dotTextureData);
            }

            return false;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}

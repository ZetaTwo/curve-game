using System;

namespace CurveGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (CurveGame game = new CurveGame())
            {
                game.Run();
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GameStateManagement;

namespace CurveGame
{
    class ZGamesScreen : SplashScreen
    {
        public ZGamesScreen()
            : base("zgames")
        {
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (TransitionPosition == 0)
            {
                LoadingScreen.Load(ScreenManager,false , new PreGameScreen());
                ExitScreen();
            }
        }
    }
}

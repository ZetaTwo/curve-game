namespace CurveGame
{
    enum GameState
    {
        Start,
        Init,
        Splash,
        Menu,
        Ingame,
        Score
    };

    enum TurnDirection
    {
        None,
        Left,
        Right
    };

    public enum PlayerNumber
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5
    };

    enum PlayerCommands
    {
        TurnLeft = 0,
        TurnRight = 1
    }
}
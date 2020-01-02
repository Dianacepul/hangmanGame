namespace HangmanMyra
{
    internal enum State
    { Welcome, Admin, Menu, Game, GameOver }

    internal abstract class GameState
    {
        public abstract State Update();

        public abstract void Render();
    }
}
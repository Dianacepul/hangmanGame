using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace HangmanMyra
{
    internal class GameOverWindow : GameState
    {
        private Game1 game1;

        private Connection connection;
        private Desktop _host;
        private State desireState = State.GameOver;
        private bool win;
        private string wordToGuess;

        public GameOverWindow(Game1 game1, Connection connection, bool win, string wordToGuess)
        {
            this.win = win;
            this.wordToGuess = wordToGuess;
            this.game1 = game1;
            MyraEnvironment.Game = game1;
            this.connection = connection;

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8,
                ShowGridLines = true
            };

            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            var restartButton = new Button
            {
                Text = "Restart",
                GridColumn = 0,
                GridRow = 2,
                Width = 200
            };
            grid.Widgets.Add(restartButton);

            restartButton.MouseDown += (s, a) =>
            {
                desireState = State.Game;
            };
            var newGameButton = new Button
            {
                Text = "Start new Game",
                GridColumn = 1,
                GridRow = 2,
                Width = 200
            };
            grid.Widgets.Add(newGameButton);
            newGameButton.MouseDown += (s, a) =>
            {
                desireState = State.Menu;
            };
            var quitButton = new Button
            {
                Text = "Quit",
                GridColumn = 2,
                GridRow = 2,
                Width = 200
            };
            grid.Widgets.Add(quitButton);
            quitButton.MouseDown += (s, a) =>
            {
                game1.Exit();
            };

            if (win == true)
            {
                var winText = new TextBlock
                {
                    Text = "YOU WIN!",
                    GridColumn = 1,
                    GridRow = 1,
                    Width = 200
                };
                grid.Widgets.Add(winText);
            }
            else
            {
                var looseText = new TextBlock
                {
                    Text = $"YOU LOST! It was {wordToGuess}",
                    GridColumn = 1,
                    GridRow = 1,
                    Width = 200
                };
                grid.Widgets.Add(looseText);
            }

            _host = new Desktop();
            _host.Widgets.Add(grid);
        }

        public override void Render()
        {
            game1.GraphicsDevice.Clear(Color.Black);

            _host.Bounds = new Rectangle(0, 0, game1.GraphicsDevice.PresentationParameters.BackBufferWidth,
              game1.GraphicsDevice.PresentationParameters.BackBufferHeight);
            _host.Render();
        }

        public override State Update()
        {
            return desireState;
        }
    }
}
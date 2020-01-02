using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace HangmanMyra
{
    internal class WelcomeWindow : GameState
    {
        private Desktop _host;
        private Game1 game1;
        private State desireState;
        private Button button;
        private Button button2;
        private TextBlock gameName;

        public WelcomeWindow(Game1 game1)
        {
            this.game1 = game1;
            MyraEnvironment.Game = this.game1;

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

            // TextBlock
            gameName = new TextBlock
            {
                Text = "HANGMAN GAME",
                GridColumn = 1,
                GridRow = 0,
                Width = 200,
            };
            grid.Widgets.Add(gameName);

            // Button
            button = new Button
            {
                GridColumn = 0,
                GridRow = 2,
                Text = "Game",
                Width = 150,
            };

            button.MouseDown += (s, a) =>
            {
                desireState = State.Menu;
            };
            grid.Widgets.Add(button);

            button2 = new Button
            {
                GridColumn = 2,
                GridRow = 2,
                Text = "Admin",
                Width = 150,
            };

            button2.MouseDown += (s, a) =>
            {
                desireState = State.Admin;
            };

            grid.Widgets.Add(button2);

            // Add it to the desktop
            _host = new Desktop();
            _host.Widgets.Add(grid);
        }

        public override State Update()
        {
            return desireState;
        }

        public override void Render()
        {
            game1.GraphicsDevice.Clear(Color.Black);

            _host.Bounds = new Rectangle(0, 0, game1.GraphicsDevice.PresentationParameters.BackBufferWidth,
              game1.GraphicsDevice.PresentationParameters.BackBufferHeight);
            _host.Render();
        }
    }
}
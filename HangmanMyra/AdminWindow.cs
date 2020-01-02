using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace HangmanMyra
{
    internal class AdminWindow : GameState
    {
        private Desktop _host;
        private Game1 game1;
        private ConnectionToAPI connectionToAPI;
        private Connection connection;
        private State desireState = State.Admin;

        public AdminWindow(Game1 game1, Connection connection, ConnectionToAPI connectionToAPI)
        {
            this.game1 = game1;
            MyraEnvironment.Game = game1;
            this.connection = connection;
           this.connectionToAPI = connectionToAPI;
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

            var addBtn = new Button
            {
                Text = "Add",
                Width = 150,
                GridColumn = 1,
                GridRow = 1
            };
            grid.Widgets.Add(addBtn);
            var inputLabel = new TextField
            {
                GridColumn = 1,
                GridRow = 0,
                Width = 150
            };
            grid.Widgets.Add(inputLabel);
            _host = new Desktop();
            _host.Widgets.Add(grid);

            addBtn.MouseDown += (s, a) =>
            {
                NewMethod(connection, connectionToAPI, inputLabel);
            };

            var backButton = new Button
            {
                Text = "Back",
                GridColumn = 1,
                GridRow = 2
            };
            grid.Widgets.Add(backButton);

            backButton.MouseDown += (s, a) =>
            {
                desireState = State.Welcome;
            };
        }

        private async void NewMethod(Connection connection, ConnectionToAPI connectionToAPI, TextField inputLabel)
        {
            connection.InsertData(inputLabel.Text, await connectionToAPI.GetWord(inputLabel.Text));
            var messageBox = Dialog.CreateMessageBox("Adding information", "Topic added secuesfully");
            messageBox.ShowModal(_host);
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
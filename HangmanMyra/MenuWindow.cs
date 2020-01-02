using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;

namespace HangmanMyra
{
    internal class MenuWindow : GameState
    {
        private Desktop _host;
        private Game1 game1;
        private Connection connection;
        private State desireState = State.Menu;
        public string Topic { get; set; }
        public string Level { get; set; }

        public MenuWindow(Game1 game1, Connection connection)
        {
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

            var topicText = new TextBlock
            {
                Text = "Select topic:",
                GridColumn = 0,
                GridRow = 1,
                Width = 100,
            };
            grid.Widgets.Add(topicText);

            var comboTopics = new ComboBox
            {
                GridColumn = 0,
                GridRow = 2
            };

            var levelText = new TextBlock
            {
                Text = "Select level:",
                GridColumn = 2,
                GridRow = 1,
                Width = 100,
            };
            grid.Widgets.Add(levelText);

            var boxLevel = new ListBox()
            {
                GridColumn = 2,
                GridRow = 2,
                Height = 100
            };

            foreach (var topic in connection.GetTopics())
            {
                comboTopics.Items.Add(new ListItem(topic));
            }
            comboTopics.SelectedIndexChanged += (sender, args) =>
            {
                Topic = comboTopics.SelectedItem.Text;
            };

            comboTopics.SelectedIndexChanged += (s, a) =>
            {
                boxLevel.Items.Clear();
                foreach (var level in connection.GetLevel(comboTopics.SelectedItem.Text))
                {
                    boxLevel.Items.Add(new ListItem(level));
                }
            };
            boxLevel.SelectedIndexChanged += (sender, args) =>
            {
                Level = boxLevel.SelectedItem.Text;
            };

            grid.Widgets.Add(comboTopics);

            grid.Widgets.Add(boxLevel);

            var playButton = new Button
            {
                GridColumn = 1,
                GridRow = 3,
                Text = "PLAY",
            };

            playButton.MouseDown += (s, a) =>
            {
                desireState = State.Game;
            };
            grid.Widgets.Add(playButton);
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
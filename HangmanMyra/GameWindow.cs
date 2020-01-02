using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanMyra
{
    internal class GameWindow : GameState
    {
        private Desktop _host;
        private SpriteBatch spriteBatch;
        private Game1 game1;
        private int x;
        private int y = 1;
        private int x2 = 1;
        private Grid gridInGrid;
        private int lives;
        public bool Win { get; set; }
        private int shownLetters;
        private List<char> lettersToChoose;
        private string choosedLevel;
        private string choosedTopic;
        public string WordToGuess { get; set; }
        private WelcomeWindow welcomeWindow;
        private Sprite gallows;
        private Sprite head;
        private Sprite body;
        private Sprite leftArm;
        private Sprite rightArm;
        private Sprite leftLeg;
        private Sprite rightLeg;
        private Sprite rope;
        private State desireState = State.Game;
        private Connection mySqlConnection;
        private Random random = new Random();
        private List<Sprite> scarecrow;
        private List<char> wrongGuessedLetters;
        private TimerCountdown timerCountdown;
        private bool timeUp;
        private TextBlock timerText;

        public GameWindow(Connection mySqlConnection, Game1 game1, string choosedTopic, string choosedLevel)
        {
            this.game1 = game1;
            wrongGuessedLetters = new List<char>();
            MyraEnvironment.Game = this.game1;
            spriteBatch = new SpriteBatch(game1.GraphicsDevice);
            welcomeWindow = new WelcomeWindow(game1);

            this.mySqlConnection = mySqlConnection;
            lettersToChoose = new List<char>();
            lettersToChoose.Add('A');
            lettersToChoose.Add('B');
            lettersToChoose.Add('C');
            lettersToChoose.Add('D');
            lettersToChoose.Add('E');
            lettersToChoose.Add('F');
            lettersToChoose.Add('G');
            lettersToChoose.Add('H');
            lettersToChoose.Add('I');
            lettersToChoose.Add('J');
            lettersToChoose.Add('K');
            lettersToChoose.Add('L');
            lettersToChoose.Add('M');
            lettersToChoose.Add('N');
            lettersToChoose.Add('O');
            lettersToChoose.Add('P');
            lettersToChoose.Add('Q');
            lettersToChoose.Add('R');
            lettersToChoose.Add('S');
            lettersToChoose.Add('T');
            lettersToChoose.Add('U');
            lettersToChoose.Add('V');
            lettersToChoose.Add('W');
            lettersToChoose.Add('X');
            lettersToChoose.Add('Y');
            lettersToChoose.Add('Z');

            lives = 7;

            this.choosedTopic = choosedTopic;
            this.choosedLevel = choosedLevel;

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8,
            };

            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Pixels, 400));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Pixels, 400));

            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Pixels, 400));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Pixels, 400));

            gridInGrid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8,
                GridRow = 0,
                GridColumn = 1
            };

            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            gridInGrid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            grid.Widgets.Add(gridInGrid);
            _host = new Desktop();
            _host.Widgets.Add(grid);

            timerCountdown = new TimerCountdown()
            {
                OnTimeOver = () => { timeUp = true; }
            };

            timerCountdown.SetTimer();
            timerText = new TextBlock
            {
                Text = "TIME LEFT: " + timerCountdown.i.ToString(),
                GridColumn = 0,
                GridRow = 0,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Top = 40,
                PaddingRight = 40
            };

            grid.Widgets.Add(timerText);

            WordToGuess = mySqlConnection.GetWords(this.choosedLevel, this.choosedTopic)[random.Next(0, mySqlConnection.GetWords(this.choosedLevel, this.choosedTopic).Count)].ToUpper();

            StringBuilder showToPlayer = new StringBuilder(WordToGuess.Length);

            for (int i = 0; i < WordToGuess.Length; i++)
            {
                if (WordToGuess[i] == ' ')
                {
                    showToPlayer.Append(' ');
                    shownLetters++;
                }
                else
                {
                    showToPlayer.Append('_');
                }
            }

            var word = new TextBlock
            {
                Text = showToPlayer.ToString(),
                GridColumn = 0,
                GridRow = 6
            };

            gridInGrid.Widgets.Add(word);

            TextBlock textOfList = new TextBlock
            {
                Text = "Full List of left letters: ",
                GridColumn = 0,
                GridRow = 0,
                Wrap = true
            };

            gridInGrid.Widgets.Add(textOfList);
            TextBlock textIncorrect = new TextBlock
            {
                Text = "Wrong guessed letters:",
                GridColumn = 0,
                GridRow = 5,
                Wrap = true
            };

            gridInGrid.Widgets.Add(textIncorrect);

            _host = new Desktop();
            _host.Widgets.Add(grid);

            foreach (var item in lettersToChoose)
            {
                gridInGrid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

                if (x >= 10)
                {
                    x = 0;
                    y++;
                }
                x++;
                var buttonOfLetters = new Button
                {
                    Text = item.ToString(),
                    GridColumn = x,
                    GridRow = y,
                };

                gridInGrid.Widgets.Add(buttonOfLetters);

                buttonOfLetters.MouseDown += (sender, args) =>
                {
                    var guess = ((Button)sender).Text[0];
                    if (WordToGuess.Contains(guess))
                    {
                        for (int i = 0; i < WordToGuess.Length; i++)
                        {
                            if (WordToGuess[i] == guess)
                            {
                                showToPlayer[i] = WordToGuess[i];
                                shownLetters++;
                            }
                        }
                        if (shownLetters == WordToGuess.Length)
                        {
                            Win = true;
                            desireState = State.GameOver;
                        }

                        word.Text = showToPlayer.ToString();
                    }
                    else
                    {
                        wrongGuessedLetters.Add(guess);

                        TextBlock wrongLetters = new TextBlock()
                        {
                            Text = guess.ToString(),

                            GridColumn = x2++,
                            GridRow = 5,
                        };

                        gridInGrid.Widgets.Add(wrongLetters);

                        lives--;
                        if (lives == 1)
                        {
                            Win = false;
                            desireState = State.GameOver;
                        }
                    }
                    gridInGrid.Widgets.Remove(buttonOfLetters);
                };
            }
        }

        public void GameContent()
        {
            scarecrow = new List<Sprite>();
            gallows = new Sprite(game1.Content.Load<Texture2D>("gallowes"));
            gallows.position.X = 3;
            gallows.position.Y = 10;
            scarecrow.Add(rope = new Sprite(game1.Content.Load<Texture2D>("rope")));
            rope.position.X = 140;
            rope.position.Y = 10;
            scarecrow.Add(head = new Sprite(game1.Content.Load<Texture2D>("head")));
            head.position.X = 120;
            head.position.Y = 45;
            scarecrow.Add(body = new Sprite(game1.Content.Load<Texture2D>("body")));
            body.position.X = 138;
            body.position.Y = 108;
            scarecrow.Add(leftArm = new Sprite(game1.Content.Load<Texture2D>("leftArm")));
            leftArm.position.X = 175;
            leftArm.position.Y = 70;
            scarecrow.Add(rightArm = new Sprite(game1.Content.Load<Texture2D>("rightArm")));
            rightArm.position.X = 81;
            rightArm.position.Y = 103;
            scarecrow.Add(leftLeg = new Sprite(game1.Content.Load<Texture2D>("leftLeg")));
            leftLeg.position.X = 120;
            leftLeg.position.Y = 156;
            scarecrow.Add(rightLeg = new Sprite(game1.Content.Load<Texture2D>("rightLeg")));
            rightLeg.position.X = 120;
            rightLeg.position.Y = 155;

            lives = lives + 1;
        }

        public override void Render()
        {
            spriteBatch.Begin();

            gallows.Draw(spriteBatch);
            for (int i = 0; i <= scarecrow.Count - lives; i++)
            {
                scarecrow[i].Draw(spriteBatch);
            }

            _host.Bounds = new Rectangle(0, 0, game1.GraphicsDevice.PresentationParameters.BackBufferWidth,
             game1.GraphicsDevice.PresentationParameters.BackBufferHeight);
            _host.Render();

            spriteBatch.End();
        }

        public override State Update()
        {
            timerText.Text = "TIME LEFT: " + timerCountdown.i.ToString();
            if (timeUp == true)
            {
                desireState = State.GameOver;
            }
            return desireState;
        }
    }
}
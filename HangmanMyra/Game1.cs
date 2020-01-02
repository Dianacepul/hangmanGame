using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;

namespace HangmanMyra
{
    internal class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Connection connection;
        private string level;
        private string topic;
        private GameState currentState;
        private ConnectionToAPI connectionToAPI;

        public Game1(Connection connection, ConnectionToAPI connectionToAPI)
        {
            this.connection = connection;
            graphics = new GraphicsDeviceManager(this);
            this.connectionToAPI = connectionToAPI;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //   welcomeWindow = new WelcomeWindow(this);
            // currentState = new MenuWindow(this, connection);

            currentState = new WelcomeWindow(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            MyraEnvironment.Game = this;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var newStateEnum = currentState.Update();
            switch (newStateEnum)
            {
                case State.Admin:
                    if (!(currentState is AdminWindow))
                        currentState = new AdminWindow(this, connection, this.connectionToAPI);
                    break;

                case State.Menu:
                    if (!(currentState is MenuWindow))
                    {
                        currentState = new MenuWindow(this, connection);
                    }
                    break;

                case State.Game:

                    if (!(currentState is GameWindow))
                    {
                        var menu = currentState as MenuWindow;
                        if (menu != null)
                        {
                            topic = menu.Topic;
                            level = menu.Level;
                        }

                        var game = new GameWindow(connection, this, topic, level);
                        game.GameContent();
                        currentState = game;

                        //cast 1
                        // currentState = new GameWindow(connection, this);
                        // ((GameWindow)currentState).GameContent();
                        //cast2
                        // (currentState as GameWindow)?.GameContent();
                    }
                    break;

                case State.GameOver:
                    if (!(currentState is GameOverWindow))
                    {
                        var game2 = currentState as GameWindow;
                        var gameOver = new GameOverWindow(this, connection, game2.Win, game2.WordToGuess);
                        currentState = gameOver;
                    }

                    break;

                case State.Welcome:
                    if (!(currentState is WelcomeWindow))
                        currentState = new WelcomeWindow(this);
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);
            spriteBatch.Begin();
            currentState.Render();
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
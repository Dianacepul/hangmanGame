using System;

namespace HangmanMyra
{
#if WINDOWS || LINUX

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Connection connection = new Connection();
            connection.Connect();
            ConnectionToAPI connectionToAPI = new ConnectionToAPI();

            using (var game = new Game1(connection, connectionToAPI))
                game.Run();
        }
    }

#endif
}
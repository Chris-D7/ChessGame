using System;
using System.Windows.Forms;

namespace ChessGame
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Chess());
        }

        public static void Restart()
        {
            Application.Restart();
        }

        public static void Exit()
        {
            Application.Exit();
        }
    }
}

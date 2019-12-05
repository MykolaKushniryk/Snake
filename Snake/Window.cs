using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class Window
    {
        #region Constants
        private const char HORIZONTAL_BORDER = '-';
        private const char VERTICAL_BORDER = '|';
        private const ConsoleColor BORDER_COLOR = ConsoleColor.DarkGray;
        private const string WIN_MESSAGE = "CONGRATULATIONS!!!";
        private const string LOSE_MESSAGE = "YOU LOSE!!!";
        private const string TRYAGAIN_BUTTON = "TRY AGAIN";
        private const ConsoleColor TRYAGAIN_BUTTON_COLOR = ConsoleColor.Green;
        private const string EXIT_BUTTON = "EXIT";
        private const ConsoleColor EXIT_BUTTON_COLOR = ConsoleColor.Red;
        private const int WINDOW_HEIGHT = 7;
        #endregion
        #region Properties
        private int MinWidth => TRYAGAIN_BUTTON.Length + EXIT_BUTTON.Length + 4;
        public readonly int X;
        public readonly int Y;
        #endregion
        #region Constructors
        public Window(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion
        #region Public Methods
        public bool Win()
        {
            Build(WIN_MESSAGE);
            return true;
        }
        public bool Lose()
        {
            Build(LOSE_MESSAGE);
            return true;
        }
        #endregion
        #region Private Methods
        private void Build(string message)
        {
            Console.ForegroundColor = BORDER_COLOR;
            Console.CursorVisible = false;

            var width = GetWidth(message);
            // Create Horizontal walls
            var horizontalWall = new string(HORIZONTAL_BORDER, width);
            Console.SetCursorPosition(X + 1, Y);
            Console.Write(horizontalWall);
            Console.SetCursorPosition(X + 1, Y + WINDOW_HEIGHT - 1);
            Console.Write(horizontalWall);

            // Create Vertical walls
            for (var i = Y; i < Y + WINDOW_HEIGHT; i++)
            {
                Console.SetCursorPosition(X, i);
                Console.Write(VERTICAL_BORDER);
                Console.SetCursorPosition(X + width + 1, i);
                Console.Write(VERTICAL_BORDER);
            }

            Console.ResetColor();
        }
        private int GetWidth(string message)
        {
            return MinWidth < message.Length + 2 ? message.Length + 2 : MinWidth;
        }
        #endregion
    }
}

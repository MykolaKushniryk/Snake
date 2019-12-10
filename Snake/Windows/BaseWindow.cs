using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Snake.Windows
{
    public abstract class BaseWindow : IDisposable
    {
        #region Constants
        private const char HORIZONTAL_BORDER = '-';
        private const char VERTICAL_BORDER = '|';
        private const ConsoleColor BORDER_COLOR = ConsoleColor.DarkGray;
        private const string TRYAGAIN_BUTTON = "TRY AGAIN";
        private const ConsoleColor TRYAGAIN_BUTTON_COLOR = ConsoleColor.Green;
        private const string EXIT_BUTTON = "EXIT";
        private const ConsoleColor EXIT_BUTTON_COLOR = ConsoleColor.Red;
        protected const int MIN_WINDOW_HEIGHT = 7;
        protected static int MIN_WINDOW_WIDTH => TRYAGAIN_BUTTON.Length + EXIT_BUTTON.Length + 6;
        #endregion
        #region Properties
        private string TRYAGAIN_BUTTON_ACTIVE = new string('-', TRYAGAIN_BUTTON.Length);
        private string EXIT_BUTTON_ACTIVE = new string('-', EXIT_BUTTON.Length);
        
        private Thread ControlThread;
        private bool Controling = true;

        protected int X;
        protected int Y;
        protected int Width;

        private bool IsExit = false;
        #endregion
        #region Constructors
        public BaseWindow(int x, int y, int width)
        {
            X = x;
            Y = y;
            Width = width;
            ControlThread = new Thread(ListenControls);
        }
        #endregion
        #region Public Methods
        #endregion
        #region Methods
        protected void Build(string message)
        {
            if (MIN_WINDOW_WIDTH > Width)
            {
                throw new ArgumentException("Width can't be less than " + MIN_WINDOW_WIDTH);
            }

            Console.ForegroundColor = BORDER_COLOR;
            Console.CursorVisible = false;

            // Create Horizontal walls
            var horizontalWall = new string(HORIZONTAL_BORDER, Width - 2);
            Console.SetCursorPosition(X + 1, Y);
            Console.Write(horizontalWall);
            Console.SetCursorPosition(X + 1, Y + MIN_WINDOW_HEIGHT - 1);
            Console.Write(horizontalWall);

            // Create Vertical walls
            for (var i = Y; i < Y + MIN_WINDOW_HEIGHT; i++)
            {
                Console.SetCursorPosition(X, i);
                Console.Write(VERTICAL_BORDER);
                Console.SetCursorPosition(X + Width - 1, i);
                Console.Write(VERTICAL_BORDER);
            }
            Console.ResetColor();

            Console.SetCursorPosition(X + 1 + GetPadding(message), Y + 2);
            Console.Write(message);

            BuildControls();
            ControlThread.Start();
        }
        protected abstract void OnSelected(bool isExite);
        #endregion
        #region Private Methods
        private void ListenControls()
        {
            while (Controling)
            {
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)
                {
                    IsExit = !IsExit;
                    BuildControls();
                }
                if (key == ConsoleKey.Enter)
                {
                    OnSelected(IsExit);
                    Controling = false;
                }
            }
        }
        private void BuildControls()
        {
            Console.SetCursorPosition(X + 2, Y + MIN_WINDOW_HEIGHT - 3);
            Console.ForegroundColor = !IsExit ? TRYAGAIN_BUTTON_COLOR : ConsoleColor.Gray;
            Console.Write(TRYAGAIN_BUTTON);
            Console.SetCursorPosition(X + 2, Y + MIN_WINDOW_HEIGHT - 2);
            Console.Write(!IsExit ? TRYAGAIN_BUTTON_ACTIVE : new string(' ', TRYAGAIN_BUTTON_ACTIVE.Length));

            Console.SetCursorPosition(X + Width - 2 - EXIT_BUTTON.Length, Y + MIN_WINDOW_HEIGHT - 3);
            Console.ForegroundColor = IsExit ? EXIT_BUTTON_COLOR : ConsoleColor.Gray;
            Console.Write(EXIT_BUTTON);
            Console.SetCursorPosition(X + Width - 2 - EXIT_BUTTON.Length, Y + MIN_WINDOW_HEIGHT - 2);
            Console.Write(IsExit ? EXIT_BUTTON_ACTIVE : new string(' ', EXIT_BUTTON_ACTIVE.Length));
            Console.ResetColor();
        }
        private int GetPadding(string message)
        {
            return (Width - message.Length - 2) / 2; 
        }
        private void Clear()
        {
            var empty = new string(' ', Width);
            for (var i = Y; i < Y + MIN_WINDOW_HEIGHT; i++)
            {
                Console.SetCursorPosition(X, i);
                Console.Write(empty);
            }
        }
        #endregion
        #region Static Methods
        protected static int GetWidth(string message)
        {
            return MIN_WINDOW_WIDTH < message.Length + 4 ? message.Length + 4 : MIN_WINDOW_WIDTH;
        }
        #endregion
        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Clear();
                }
                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

using System;

namespace Snake.Events
{
    public class SnakeEventArgs : EventArgs
    {
        #region Properties
        public readonly Snake Snake;
        #endregion
        #region Constructors
        public SnakeEventArgs(Snake snake)
        {
            Snake = snake;
        }
        #endregion
    }
}

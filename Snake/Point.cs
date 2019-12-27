using System;

namespace Snake
{
    public abstract class Point
    {
        #region Properties
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public char Symbol { get; private set; }
        #endregion
        #region Constructors
        public Point(int x, int y, char symbol)
        {
            X = x;
            Y = y;
            Symbol = symbol;
        }
        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
            Symbol = point.Symbol;
        }
        #endregion
        #region Public Methods
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void Draw()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Symbol);
        }
        public void Clear()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
        public bool IsHit(Point point)
        {
            return X == point.X && Y == point.Y;
        }
        #endregion
    }
}

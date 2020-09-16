using System;

namespace Snake
{
    public abstract class Point
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public char Symbol { get; private set; }

        public Point(int x, int y, char symbol)
        {
            X = x;
            Y = y;
            Symbol = symbol;
        }

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
    }
}

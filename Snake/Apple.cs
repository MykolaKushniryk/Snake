using System;
using System.Linq;
using Snake.Interfaces;

namespace Snake
{
    public class Apple : IApple
    {
        #region Constants
        public const char AppleSymbol = '$';
        public const ConsoleColor AppleColor = ConsoleColor.Green;
        #endregion
        #region Private Properties
        private readonly IArea Area;
        private readonly Random Random;
        #endregion
        #region Properties
        public int X { get; private set; }
        public int Y { get; private set; }
        #endregion
        #region Constructors
        public Apple(IArea area, IBlankSpace blankSpace)
        {
            Area = area;
            Random = new Random();
            Refresh(blankSpace);
        }
        #endregion
        #region Public Methods
        public void Display()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = AppleColor;
            Console.Write(AppleSymbol);
            Console.ResetColor();
        }
        public void Refresh(IBlankSpace blankSpace)
        {
            var xy = CreateApple(blankSpace);
            X = xy.Item1;
            Y = xy.Item2;
        }
        #endregion
        #region Private Methods
        private (int, int) CreateApple(IBlankSpace blankSpace)
        {
            var xy = (Random.Next(Area.StartX, Area.EndX), Random.Next(Area.StartY, Area.EndY));
            while (blankSpace.Coordinates.Any(body => body.Item1 == xy.Item1 && body.Item2 == xy.Item2))
            {
                xy = (Random.Next(Area.StartX, Area.EndX), Random.Next(Area.StartY, Area.EndY));
            }
            return xy;
        }
        #endregion
    }
}

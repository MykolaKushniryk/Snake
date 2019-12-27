using System;
using System.Linq;
using Snake.Interfaces;

namespace Snake
{
    public class Apple : Point
    {
        #region Constants
        public const char SYMBOL = '$';
        public const ConsoleColor COLOR = ConsoleColor.Green;
        #endregion
        #region Private Properties
        private readonly IArea Area;
        private readonly Random Random;
        #endregion
        #region Constructors
        public Apple(IArea area, IAreaObject areaObject) : base(0, 0, SYMBOL)
        {
            Area = area;
            Random = new Random();
            Refresh(areaObject);
        }
        #endregion
        #region Public Methods
        public void Display()
        {
            Console.ForegroundColor = COLOR;
            base.Draw();
            Console.ResetColor();
        }
        public void Refresh(IAreaObject areaObject)
        {
            var xy = Create(areaObject);
            X = xy.Item1;
            Y = xy.Item2;
        }
        #endregion
        #region Private Methods
        private (int, int) Create(IAreaObject areaObject)
        {
            var xy = (Random.Next(Area.StartX, Area.EndX), Random.Next(Area.StartY, Area.EndY));
            while (areaObject.Objects.Any(body => body.Item1 == xy.Item1 && body.Item2 == xy.Item2))
            {
                xy = (Random.Next(Area.StartX, Area.EndX), Random.Next(Area.StartY, Area.EndY));
            }
            return xy;
        }
        #endregion
    }
}

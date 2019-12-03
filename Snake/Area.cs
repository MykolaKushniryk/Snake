using Snake.Interfaces;

namespace Snake
{
    public class Area : IArea
    {
        #region Properties
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        #endregion
        #region Constructors
        public Area(int x, int y, int width, int height)
        {
            StartX = x;
            StartY = y;
            Width = width;
            Height = height;
            EndX = StartX + Width - 1;
            EndY = StartY + Height - 4;
        }
        #endregion
    }
}

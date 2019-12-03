namespace Snake.Interfaces
{
    public interface IArea
    {
        #region Properties
        int StartX { get; }
        int StartY { get; }
        int EndX { get; }
        int EndY { get; }
        int Width { get; }
        int Height { get; }
        #endregion
    }
}

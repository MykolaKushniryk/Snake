namespace Snake.Interfaces
{
    public interface IMap
    {
        bool IsWall(int x, int y);
        bool InMap(int x, int y);
    }
}

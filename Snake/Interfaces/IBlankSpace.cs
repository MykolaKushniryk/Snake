using System.Collections.Generic;

namespace Snake.Interfaces
{
    public interface IBlankSpace
    {
        IEnumerable<(int, int)> Coordinates { get; }
    }
}

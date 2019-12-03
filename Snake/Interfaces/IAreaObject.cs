using System.Collections.Generic;

namespace Snake.Interfaces
{
    public interface IAreaObject
    {
        IEnumerable<(int, int)> Objects { get; }
    }
}

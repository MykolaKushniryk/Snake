using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Interfaces
{
    public interface ISnake
    {
        void Start();
        void Stop();
        void AddBody();
        void EatBody(int bodyIndex);
    }
}

using System;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            var map = new Map(0, 0, 80, 20, "mkushniryk");
            map.Init();
            Console.ReadKey();
            var snake = new Snake(6, 6);
            snake.Start();
            Console.ReadKey();
        }
    }
}

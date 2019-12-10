using System;
using Snake.Windows;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            new Game().PlayGame();
            Console.ReadKey();
        }
    }
}

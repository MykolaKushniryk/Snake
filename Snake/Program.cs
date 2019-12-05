using System;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Game().Play();
            new Window(9, 9).Win();
            Console.ReadKey();
        }
    }
}

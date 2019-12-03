using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Snake.Events;
using Snake.Interfaces;

namespace Snake
{
    public class Snake : ISnake, IBlankSpace
    {
        #region Constants
        public const int DEFAULT_LENGHT = 4;
        public const ConsoleColor HEAD_COLOR = ConsoleColor.White;
        public const ConsoleColor DEFAULT_COLOR = ConsoleColor.Gray;
        public const char SNAKE_BODY = '*';
        public int DELAY = 100;
        #endregion
        #region Public Properties
        public IEnumerable<(int, int)> Coordinates
        {
            get
            {
                var collection = new List<(int, int)>
                {
                    (Head.X, Head.Y)
                };
                foreach (var body in Tail)
                {
                    collection.Add((body.X, body.Y));
                }
                return collection;
            }
        }
        #endregion
        #region Control Properties
        private readonly Thread MovingThread;
        private readonly Thread ControlingThread;
        private volatile int Direction = 4;
        private volatile bool IsMoving = true;
        #endregion
        #region Snake Properties
        private readonly int StartX;
        private readonly int StartY;
        private SnakeBody Head;
        private readonly List<SnakeBody> Tail;
        #endregion
        #region Area Properties
        private readonly IArea Area;
        #endregion
        #region Constructors
        public Snake(IArea area) : this(DEFAULT_LENGHT, area) { }
        public Snake(int lenght, IArea area)
        {
            Area = area;

            Head = new SnakeBody { X = 16, Y = 6 };
            Tail = new List<SnakeBody>();

            InitBody(10);
            
            MovingThread = new Thread(Moving);
            ControlingThread = new Thread(Controling);
            thread = new Thread(DisplayFill);
            Console.CursorVisible = false;

            void InitBody(int lenght)
            {
                if (lenght == 0)
                {
                    throw new ArgumentException("Snake lenght can't be zero!");
                }
                for (var i = lenght - 1; i > 0; i--)
                {
                    Tail.Add(new SnakeBody { X = i, Y = StartY });
                }
            }
        }
        #endregion
        #region Public Methods
        public Snake Build()
        {

            DisplayInit();
            return this;
        }
        private Apple Apple;
        public void Start(Apple apple)
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //InitialDispaly();
            Apple = apple;
            ControlingThread.Start();
            MovingThread.Start();
            thread.Start();
        }
        public void Stop()
        {
            MovingThread.Abort();
            ControlingThread.Abort();
        }
        public void AddBody()
        {
            var body = new SnakeBody { };
            Tail.Add(body);
        }
        public void EatBody(int position)
        {

            throw new NotImplementedException();
        }
        #endregion
        #region Events
        public event SendMessage AppleAchieved;
        public delegate void SendMessage(IBlankSpace blankSpace);
        private void OnAppleAchieved()
        {
            DELAY -= 2;
            AppleAchieved?.Invoke(this);
        }
        #endregion
        #region Private Methods
        private Thread thread;

        

        private void Controling()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (IsMoving)
                {
                    if (key == ConsoleKey.LeftArrow && Direction != 3)
                    {
                        Direction = 1;
                    }
                    else if (key == ConsoleKey.UpArrow && Direction != 4)
                    {
                        Direction = 2;
                    }
                    else if (key == ConsoleKey.RightArrow && Direction != 1)
                    {
                        Direction = 3;
                    }
                    else if (key == ConsoleKey.DownArrow && Direction != 2)
                    {
                        Direction = 4;
                    }
                }
            }
        }
        private void Moving()
        {
            while (true)
            {
                if (IsMoving)
                {
                    var x = getX();
                    var y = getY();
                    MoveNext(x, y);
                    if (x == Apple.X && y == Apple.Y)
                    {
                        OnAppleAchieved();
                    }
                    Thread.Sleep(DELAY);
                }
            }
            int getX()
            {
                var x = Head.X + (Direction == 1 ? -1 : Direction == 3 ? 1 : 0);
                if (x < Area.StartX)
                {
                    return Area.EndX;
                }
                if (x > Area.EndX)
                {
                    return Area.StartX;
                }
                return x;
            }
            int getY()
            {
                var y = Head.Y + (Direction == 2 ? -1 : Direction == 4 ? 1 : 0);
                if (y < Area.StartY)
                {
                    return Area.EndY;
                }
                if (y > Area.EndY)
                {
                    return Area.StartY;
                }
                return y;
            }
        }
        private void MoveNext(int x, int y)
        {
            Tail.Insert(0, Head);
            Head = Tail[Tail.Count - 1];
            DisplayNext(x, y, Head.X, Head.Y);

            Head.X = x;
            Head.Y = y;

            Tail.RemoveAt(Tail.Count - 1);
        }
        private void DisplayInit()
        {
            Console.ForegroundColor = HEAD_COLOR;
            Console.SetCursorPosition(Head.X, Head.Y);
            Console.Write(SNAKE_BODY);
            Console.ForegroundColor = DEFAULT_COLOR;

            foreach (var body in Tail)
            {
                Console.SetCursorPosition(body.X, body.Y);
                Console.Write(SNAKE_BODY);
            }
        }
        private void DisplayNext(int headX, int headY, int tailX, int tailY)
        {
            Console.ForegroundColor = HEAD_COLOR;
            Console.SetCursorPosition(headX, headY);
            Console.Write(SNAKE_BODY);
            Console.ForegroundColor = DEFAULT_COLOR;

            Console.SetCursorPosition(Tail[0].X, Tail[0].Y);
            Console.Write(SNAKE_BODY);
            Console.SetCursorPosition(tailX, tailY);
            Console.Write(' ');
        }
        private void DisplayFill()
        {
            Thread.Sleep(1000);
            IsMoving = false;
            var first = true;
            SnakeBody prev = new SnakeBody();
            foreach (var body in Tail)
            {
                if (!first)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(prev.X, prev.Y);
                    Console.Write(SNAKE_BODY);
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(body.X, body.Y);
                Console.Write(SNAKE_BODY);

                first = false;
                prev = body;
                Thread.Sleep(50);
            }
            Thread.Sleep(50);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(prev.X, prev.Y);
            Console.Write(SNAKE_BODY);
            Thread.Sleep(100);
            IsMoving = true;
        }
        #endregion        
    }
    public class SnakeBody
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

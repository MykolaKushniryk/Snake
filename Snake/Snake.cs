using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Snake.Interfaces;

namespace Snake
{
    public class Snake : IAreaObject
    {
        #region Constants
        public const int DEFAULT_LENGHT = 4;
        public const ConsoleColor HEAD_COLOR = ConsoleColor.White;
        public const ConsoleColor DEFAULT_COLOR = ConsoleColor.Gray;
        public const ConsoleColor FILL_COLOR = ConsoleColor.Green;
        public const char SNAKE_BODY = '*';
        public int DELAY = 100;
        #endregion
        #region Public Properties
        public bool UseArea { get; private set; } = true;
        public IEnumerable<(int, int)> Objects
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
        private volatile Directions Direction = Directions.RIGHT;
        private volatile bool IsMoving = true;
        private volatile bool IsStop = false;
        public bool IsError = false;
        #endregion
        #region Snake Properties
        private readonly int StartX;
        private readonly int StartY;
        private Body Head;
        private readonly List<Body> Tail;
        private Apple Apple;
        #endregion
        #region Area Properties
        private readonly IArea Area;
        #endregion
        #region Constructors
        public Snake(IArea area) : this(DEFAULT_LENGHT, area) { }
        public Snake(int lenght, IArea area)
        {
            Area = area;
            StartY = 6;
            Head = new Body(16, 6, SNAKE_BODY);
            Tail = new List<Body>();

            InitBody(lenght);

            MovingThread = new Thread(Move);
            ControlingThread = new Thread(Control);
            Console.CursorVisible = false;

            void InitBody(int lenght)
            {
                if (lenght == 0)
                {
                    throw new ArgumentException("Snake lenght can't be zero!");
                }
                for (var i = 0; i < lenght - 1; i++)
                {
                    Tail.Add(new Body(15 - i, StartY, SNAKE_BODY));
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
        public void Start(Apple apple)
        {
            Apple = apple;
            ControlingThread.Start();
            MovingThread.Start();
        }
        public void Stop()
        {
            IsStop = true;
        }
        #endregion
        #region Events
        public event SendMessage AppleAchieved;
        public delegate void SendMessage(IAreaObject areaObject);
        private void OnAppleAchieved()
        {
            if (DELAY > 0)
            {
                DELAY -= 2;
            }
            AppleAchieved?.Invoke(this);
            var last = Tail[Tail.Count() - 1];
            Tail.Add(new Body(last.X, last.Y, SNAKE_BODY));
        }
        #endregion
        #region Private Methods
        private void Control()
        {
            while (!IsStop)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (IsMoving)
                    {
                        if (key == ConsoleKey.LeftArrow && Direction != Directions.RIGHT)
                        {
                            Direction = Directions.LEFT;
                        }
                        else if (key == ConsoleKey.UpArrow && Direction != Directions.DOWN)
                        {
                            Direction = Directions.UP;
                        }
                        else if (key == ConsoleKey.RightArrow && Direction != Directions.LEFT)
                        {
                            Direction = Directions.RIGHT;
                        }
                        else if (key == ConsoleKey.DownArrow && Direction != Directions.UP)
                        {
                            Direction = Directions.DOWN;
                        }
                    }
                }
            }
            return;
        }
        private void Move()
        {
            while (!IsStop)
            {
                if (IsMoving)
                {
                    var x = getX();
                    var y = getY();
                    if (x == Apple.X && y == Apple.Y)
                    {
                        OnAppleAchieved();
                        MoveNext(x, y);
                        //DisplayFill();
                    }
                    else if (Tail.Any(body => body.X == x && body.Y == y))
                    {
                        IsError = true;
                        return;
                    }
                    else
                    {
                        MoveNext(x, y);
                    }
                    Thread.Sleep(DELAY);
                }
            }
            return;
            int getX()
            {
                var x = Head.X + (Direction == Directions.LEFT ? -1 : Direction == Directions.RIGHT ? 1 : 0);
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
                var y = Head.Y + (Direction == Directions.UP ? -1 : Direction == Directions.DOWN ? 1 : 0);
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
            Tail[0].Display(DEFAULT_COLOR);

            Head = Tail[Tail.Count - 1];
            Tail.RemoveAt(Tail.Count - 1);

            Head.Clear();
            Head.SetPosition(x, y);
            Head.Display(HEAD_COLOR);
        }
        private void DisplayInit()
        {
            Head.Display(HEAD_COLOR);
            Tail.ForEach(body => body.Display(DEFAULT_COLOR));
        }
        private void DisplayFail()
        {

        }

        private void DisplayFill()
        {
            var delay = 25;
            IsMoving = false;
            var first = true;
            var prev = new Body(0, 0, SNAKE_BODY);
            foreach (var body in Tail)
            {
                if (!first)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.SetCursorPosition(prev.X, prev.Y);
                    Console.Write(SNAKE_BODY);
                }

                Console.ForegroundColor = FILL_COLOR;
                Console.SetCursorPosition(body.X, body.Y);
                Console.Write(SNAKE_BODY);

                first = false;
                prev = body;
                Thread.Sleep(delay);
            }
            Thread.Sleep(delay);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(prev.X, prev.Y);
            Console.Write(SNAKE_BODY);
            IsMoving = true;
        }
        #endregion 
    }
    public class SnakeBody
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Body : Point
    {
        #region Constants

        #endregion
        public Body(int x, int y, char symbol) : base(x, y, symbol)
        {

        }

        #region Public Methods
        public void Move(Directions direction, int step = 1)
        {
            switch (direction)
            {
                case Directions.RIGHT:
                    X += step;
                    break;
                case Directions.LEFT:
                    X -= step;
                    break;
                case Directions.UP:
                    Y -= step;
                    break;
                case Directions.DOWN:
                    Y += step;
                    break;
            }
        }
        public void Display(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            base.Draw();
        }
        #endregion
    }

    enum Directions
    {
        LEFT = 0,
        UP = 1,
        RIGHT = 2,
        DOWN = 3
    }
}

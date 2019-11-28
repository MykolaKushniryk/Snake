using Snake.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    public class Snake : ISnake
    {
        #region Constants
        public const int DEFAULT_LENGHT = 4;
        public const ConsoleColor HEAD_COLOR = ConsoleColor.Yellow;
        #endregion
        #region Properties
        private readonly int StartX;
        private readonly int StartY;
        private readonly Thread MovingThread;
        private readonly Thread ControlThread;
        private volatile int Direction = 3;

        private SnakeBody Head;
        private readonly List<SnakeBody> Tail;
        #endregion
        #region Constructors
        public Snake(int x, int y) : this(x, y, DEFAULT_LENGHT)
        {

        }
        public Snake(int x, int y, int lenght)
        {
            StartX = x;
            StartY = y;
            Head = new SnakeBody { Position = 0, X = StartX, Y = StartY };
            Tail = new List<SnakeBody>();

            InitBody(lenght);

            MovingThread = new Thread(Moving);
            ControlThread = new Thread(Controls);
            Console.CursorVisible = false;
        }
        #endregion


        #region Indexer
        public SnakeBody this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return Head;
                }
                foreach (var body in Tail)
                {
                    if (body.Position == i)
                    {
                        return body;
                    }
                }
                return null;
            }
        }
        #endregion
        #region Public Methods
        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            InitialDispaly();

            ControlThread.Start();
            MovingThread.Start();
        }
        public void Stop()
        {
            MovingThread.Abort();
            ControlThread.Abort();
        }
        public void AddBody()
        {
            var body = new SnakeBody { Position = Tail.Count(), };
            Tail.Add(body);
        }
        public void EatBody(int position)
        {

            throw new NotImplementedException();
        }
        #endregion
        #region Private Methods
        private void InitBody(int lenght)
        {
            if (lenght == 0)
            {
                throw new ArgumentException("Snake lenght can't be zero!");
            }
            var position = 1;
            for (var i = lenght - 1; i > 0; i--)
            {
                Tail.Add(new SnakeBody { Position = position++, X = i, Y = 6 });
            }
        }
        private void InitDisplay()
        {

        }
        #endregion

        public void Controls()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    Direction = 1;
                }
                else if (key == ConsoleKey.UpArrow) 
                {
                    Direction = 2;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    Direction = 3;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    Direction = 4;
                }
            }
        }
        public void Moving()
        {
            while (true)
            {
                Move();
                Thread.Sleep(100);
            }
        }

        // 1 2 3 4
        

        public void Move()
        {
            Tail.Insert(0, Head);
            var x = Head.X;
            var y = Head.Y;

            var tail = Tail[Tail.Count - 1];
            var tailX = tail.X;
            var tailY = tail.Y;
            Head = tail;
            Head.X = x;
            Head.Y = y;
            Head.X += Direction == 1 ? -1 : Direction == 3 ? 1 : 0;
            Head.Y += Direction == 2 ? -1 : Direction == 4 ? 1 : 0;
            MoveDisplay(Head.X, Head.Y, tailX, tailY);
            Tail.RemoveAt(Tail.Count - 1);
        }

        private void InitialDispaly()
        {
            Console.SetCursorPosition(Head.X, Head.Y);
            Console.Write('*');

            foreach (var body in Tail)
            {
                Console.SetCursorPosition(body.X, body.Y);
                Console.Write('*');
            }
        }
        private void MoveDisplay(int headX, int headY, int tailX, int tailY)
        {
            Console.SetCursorPosition(headX, headY);
            Console.Write('*');
            Console.SetCursorPosition(tailX, tailY);
            Console.Write(' ');
        }

        
    }
    public class SnakeBody
    {
        public int Position { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}

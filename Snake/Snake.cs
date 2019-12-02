using Snake.Events;
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
        public const ConsoleColor HEAD_COLOR = ConsoleColor.White;
        public const ConsoleColor DEFAULT_COLOR = ConsoleColor.Gray;
        public const char SNAKE_BODY = '*';
        public const int DELAY = 75;
        #endregion
        #region Properties
        private readonly int StartX;
        private readonly int StartY;
        private readonly Thread MovingThread;
        private readonly Thread ControlingThread;
        private volatile int Direction = 3;
        private bool IsMoving = true;

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
            Head = new SnakeBody { X = StartX, Y = StartY };
            Tail = new List<SnakeBody>();

            InitBody(10);
            DisplayInit();

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


        #region Indexer
        public SnakeBody this[int i]
        {
            get
            {
                if (i == 0)
                {
                    return Head;
                }

                if (Tail.Count() > i - 2 && i - 2 < 0)
                {
                    return Tail[i];
                }
                return null;
            }
        }
        #endregion
        #region Public Methods
        public void Start()
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //InitialDispaly();

            ControlingThread.Start();
            MovingThread.Start();
            thread.Start();
        }
        public void Suspend()
        {
            ControlingThread.Suspend();
            MovingThread.Suspend();
        }
        public void Resume()
        {
            ControlingThread.Resume();
            MovingThread.Resume();
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
        public event EventHandler<SnakeEventArgs> ShapeChanged;
        private void OnSnakeMoved(SnakeEventArgs e)
        {
            ShapeChanged?.Invoke(this, e);
        }
        private void HandleWallCrossed(object sender, PositionEventArgs e)
        {
            //Shape s = (Shape)sender;

            // Diagnostic message for demonstration purposes.
            //Console.WriteLine("Received event. Shape area is now {0}", e.NewArea);

            //// Redraw the shape here.
            //s.Draw();
        }
        #endregion
        #region Private Methods
        private Thread thread;
        private void Controling()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
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
        private void Moving()
        {
            while (true)
            {
                if (IsMoving)
                {
                    var x = Head.X + (Direction == 1 ? -1 : Direction == 3 ? 1 : 0);
                    var y = Head.Y + (Direction == 2 ? -1 : Direction == 4 ? 1 : 0);
                    MoveNext(x, y);
                    Thread.Sleep(DELAY);
                }
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

using System;
using Snake.Interfaces;

namespace Snake
{
    public class Map
    {
        private const char HorizontalWall = '-';
        private const char VerticalWall = '|';
        private const ConsoleColor WallColor = ConsoleColor.DarkGray;
        private const string ScoreTitle = "SCORE:";
        private const string LenghtTitle = "LENGHT:";
        private const string NicknameTitle = "NICKNAME:";
        private const int MaxScore = 100000;
        private const ConsoleColor INFO_COLOR = ConsoleColor.Green;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public IArea Area => area.Value;

        private readonly int MapX;
        private readonly int MapY;

        private int Score = 0;
        private readonly int ScoreX;
        private readonly int ScoreY;

        private readonly int LenghtX;
        private readonly int LenghtY;

        private readonly string Nickname;
        private readonly int NicknameX;
        private readonly int NicknameY;
        private int Len = Snake.DEFAULT_LENGHT;

        private readonly Lazy<IArea> area;

        public Map(int x, int y, int width, int height, string nickname)
        {
            MapX = x;
            MapY = y;
            Width = width;
            Height = height + 4;
            Nickname = nickname;
            ScoreX = MapX + ScoreTitle.Length + 3;
            ScoreY = MapY + 2;

            LenghtX = ScoreX + LenghtTitle.Length + MaxScore.ToString().Length + 2;
            LenghtY = ScoreY;

            Nickname = nickname;
            NicknameX = MapX + Width - Nickname.Length - 2;
            NicknameY = MapY + 2;

            area = new Lazy<IArea>(new Area(MapX + 1, MapY + 5, Width - 2, Height - 2));
        }

        public Map Build()
        {
            CreateWalls();
            CreateTitle();
            return this;
        }
        public void AddScore(int points)
        {
            Score += points;
            SetScore(Score);
        }
        public void AddLenght(int lenght)
        {
            Len += lenght;
            SetSnakeLenght(Len);
        }
        public void SetSnakeLenght(int lenght)
        {
            Console.SetCursorPosition(LenghtX, LenghtY);
            Console.ForegroundColor = INFO_COLOR;
            Console.Write(lenght + new string(' ', 3));
        }

        private void CreateWalls()
        {
            Console.ForegroundColor = WallColor;
            Console.CursorVisible = false;

            // Create Horizontal walls
            var horizontalWall = new string(HorizontalWall, MapX + Width - 2);
            Console.SetCursorPosition(MapX + 1, MapY);
            Console.Write(horizontalWall);
            Console.SetCursorPosition(MapX + 1, MapY + 4);
            Console.Write(horizontalWall);
            Console.SetCursorPosition(MapX + 1, MapY + Height);
            Console.Write(horizontalWall);

            // Create Vertical walls
            for (var i = MapY; i <= MapY + Height; i++)
            {
                Console.SetCursorPosition(MapX, i);
                Console.Write(VerticalWall);
                Console.SetCursorPosition(MapX + Width - 1, i);
                Console.Write(VerticalWall);
            }

            Console.ResetColor();
        }
        private void CreateTitle()
        {
            // Set Score
            Console.SetCursorPosition(MapX + 2, MapY + 2);
            Console.Write(ScoreTitle);
            SetScore(Score);

            // Set Lenght
            Console.SetCursorPosition(LenghtX - LenghtTitle.Length - 1, LenghtY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(LenghtTitle);
            SetSnakeLenght(Snake.DEFAULT_LENGHT);

            // Set Nickname
            Console.SetCursorPosition(NicknameX - NicknameTitle.Length - 1, NicknameY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(NicknameTitle);
            Console.SetCursorPosition(NicknameX, NicknameY);
            Console.ForegroundColor = INFO_COLOR;
            Console.Write(Nickname);
        }
        private void SetScore(int score)
        {
            Console.SetCursorPosition(ScoreX, ScoreY);
            Console.ForegroundColor = INFO_COLOR;
            Console.Write(score + new string(' ', MaxScore.ToString().Length - score.ToString().Length));
        }
    }
}
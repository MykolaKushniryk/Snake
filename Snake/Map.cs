using System;
using Snake.Interfaces;

namespace Snake
{
    public class Map : IMap
    {
        #region Constants
        private const char HorizontalWall = '-';
        private const char VerticalWall = '|';
        private const ConsoleColor WallColor = ConsoleColor.DarkGray;
        private const string ScoreTitle = "SCORE:";
        private const string LenghtTitle = "LENGHT:";
        private const string NicknameTitle = "NICKNAME:";
        private const int MaxScore = 100000;
        #endregion
        #region Properties
        private readonly int MapX;
        private readonly int MapY;
        public readonly int Width;
        public readonly int Height;
       
        private int Score = 0;
        private readonly int ScoreX;
        private readonly int ScoreY;

        private readonly int LenghtX;
        private readonly int LenghtY;

        private readonly string Nickname;
        private readonly int NicknameX;
        private readonly int NicknameY;
        #endregion
        #region Constructors
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
        }
        #endregion
        #region Public Methods
        public void Init()
        {
            CreateWalls();
            CreateTitle();
        }
        public bool IsWall(int x, int y)
        {
            return (MapX == x || (MapX + Width - 1) == x) && (MapY == y || MapY + 4 == y || MapY + Height == y);
        }
        public bool InMap(int x, int y)
        {
            return (MapX < x && x < MapX + Width - 1) && (MapY + 4 < y && y < MapY + Width - 1);
        }
        public void AddPoints(int points)
        {
            Score += points;
            SetScore(Score);
        }
        public void SetSnakeLenght(int lenght)
        {
            Console.SetCursorPosition(LenghtX, LenghtY);
            Console.Write(lenght + new string(' ', 3));
        }
        #endregion
        #region Private Methods
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
            Console.Write(LenghtTitle);
            SetSnakeLenght(0);

            // Set Nickname
            Console.SetCursorPosition(NicknameX - NicknameTitle.Length - 1, NicknameY);
            Console.Write(NicknameTitle);
            Console.SetCursorPosition(NicknameX, NicknameY);
            Console.Write(Nickname);
        }
        private void SetScore(int score)
        {
            Console.SetCursorPosition(ScoreX, ScoreY);
            Console.Write(score + new string(' ', MaxScore.ToString().Length - score.ToString().Length));
        }
        #endregion
    }
}

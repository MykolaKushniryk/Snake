using Snake.Interfaces;
using Snake.Windows;
using System;

namespace Snake
{
    public class Game
    {
        #region Game Properties
        private int Score = 0;
        private int WinScore = 10;
        #endregion
        #region Game Objects
        private Map Map;
        private Apple Apple;
        private Snake Snake;
        #endregion
        #region Constructors
        public Game()
        {

        }
        #endregion
        #region Public Methods
        public void PlayGame()
        {
            while (true)
            {
                Console.Clear();
                var res = Play();

                if (res)
                {
                    using var window = new WinWindow(9, 9);
                    if (!window.Create().TryAgain()) break;
                    Score = 0;
                }
                else
                {
                    using var window = new LoseWindow(9, 9);
                    if (!window.Create().TryAgain()) break;
                    Score = 0;
                }
            }
            Console.Clear();
            Console.WriteLine("Goodbye!");
            
        }
        public bool Play()
        {
            Map = new Map(0, 0, 80, 20, "mkushniryk").Build();
            
            Snake = new Snake(Map.Area);
            Apple = new Apple(Map.Area, Snake);
            try
            {
                Apple.Display();
                Snake.AppleAchieved += Refresh;
                Snake.Build().Start(Apple);
                while (Score <= WinScore && !Snake.IsError)
                {

                }
                return !Snake.IsError;
            }
            catch (Exception)
            {
                Score = 0;
                return false;
            }
            finally
            {
                Snake.Stop();
            }
        }
        
        private void Refresh(IAreaObject areaObject)
        {
            Map.AddScore(1);
            Score++;
            Map.AddLenght(1);
            Apple.Refresh(areaObject);
            Apple.Display();
        }
        #endregion
    }
}

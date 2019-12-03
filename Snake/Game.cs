using Snake.Interfaces;

namespace Snake
{
    public class Game
    {
        #region Properties

        #endregion
        #region Constructors
        public Game()
        {

        }
        #endregion
        #region Public Methods
        public void Play()
        {
            Map = new Map(0, 0, 80, 20, "mkushniryk").Build();

            Snake = new Snake(Map.Area);
            Apple = new Apple(Map.Area, Snake);
            Apple.Display();
            Snake.AppleAchieved += Refresh;
            Snake.Build().Start(Apple);
        }
        private Map Map;
        private Apple Apple;
        private Snake Snake;
        private void Refresh(IAreaObject areaObject)
        {
            Map.AddPoints(1);
            Map.AddLenght(1);
            Apple.Refresh(areaObject);
            Apple.Display();
        }
        #endregion
    }
}

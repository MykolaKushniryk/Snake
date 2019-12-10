using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Windows
{
    public class WinWindow : BaseWindow
    {
        #region Constants
        private const string MESSAGE = "CONGRATULATIONS!!!";
        private static int WIDTH => GetWidth(MESSAGE);
        #endregion
        #region Properties
        private bool Selected = false;
        private bool IsExite;
        #endregion
        #region Constructors
        public WinWindow(int x, int y) : base(x, y, WIDTH) { }
        #endregion
        #region Public Methods
        public WinWindow Create()
        {
            base.Build(MESSAGE);
            return this;
        }
        public bool TryAgain()
        {
            while (!Selected)
            {

            }
            return !IsExite;
        }
        #endregion
        #region Private Methods
        protected override void OnSelected(bool isExite)
        {
            Selected = true;
            IsExite = isExite;
        }
        #endregion
    }
}

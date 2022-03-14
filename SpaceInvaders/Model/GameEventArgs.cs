using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    internal class GameEventArgs : EventArgs
    {
        //játék vége eseményhez
        private int _score;
        private int _lives;
        private int _yPos;

        //pontszám lekérdezése
        public int Score { get { return _score; } }
        public int Lives { get { return _lives; } }
        public int YPos { get { return _yPos; } }

        #region Constructor
        public GameEventArgs(int score, int lives, int yPos)
        {
            _score = score;
            _lives = lives;
            _lives = lives;
            _yPos = yPos;
        }
        #endregion
    }
}

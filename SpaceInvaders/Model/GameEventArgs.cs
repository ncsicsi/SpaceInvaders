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
        private int _xPos;

        //pontszám lekérdezése
        public int Score { get { return _score; } }
        public int Lives { get { return _lives; } }
        public int YPos { get { return _xPos; } }

        #region Constructor
        public GameEventArgs(int score, int lives, int xPos)
        {
            _score = score;
            _lives = lives;
            _lives = lives;
            _xPos = xPos;
        }
        #endregion
    }
}

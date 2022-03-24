using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class GameEventArgs : EventArgs
    {
        //játék vége eseményhez
        private int _score;
        private int _lives;
        private int _xPos;
        private Bullet[] _bullets = new Bullet[15];

        //pontszám lekérdezése
        public int Score { get { return _score; } }
        public int Lives { get { return _lives; } }
        public int YPos { get { return _xPos; } }
        public Bullet[] Bullets { get { return _bullets; } }

        #region Constructor
        public GameEventArgs(int score, int lives, int xPos, Bullet[] bullets)
        {
            _score = score;
            _lives = lives;
            _xPos = xPos;
            _bullets = bullets;
        }
        #endregion
    }
}

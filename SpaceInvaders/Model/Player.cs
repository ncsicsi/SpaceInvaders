using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    internal class Player
    {
        #region Fields
        private static int _width = 60;
        private static int _speed = 5;
        private static int _yPos = 590;
        private int _xPos;
        private bool _goLeft;
        private bool _goRight;
        private bool _bullet;
        private int _score;
        private int _lives;
        #endregion

        #region Property
        public int Width { get { return _width; } }
        public int Speed { get { return _speed; } }
        public int YPos { get { return _yPos; } }
        public int XPos { get { return _xPos; } set {_xPos = value;} }
        public bool GoLeft { get { return _goLeft; } set { _goLeft = value;} }
        public bool GoRight { get { return _goRight; } set { _goRight = value;} }
        public bool Bullet { get { return _bullet; } set {_bullet = value; } }
        public int Score { get { return _score; } set { _score = value;} }
        public int Lives { get { return _lives; } set { _lives = value;} }
        #endregion

        #region Public Methods

        public void Move(int windowBorder, int windowWidth)
        {
            if (_goLeft && _xPos > windowBorder)
            {
                _xPos -= _speed;
            }
            else if (_goRight && _xPos + _width + windowBorder + _speed < windowWidth)
            {
                _xPos += _speed;
            }
        }

        #endregion
    }
}

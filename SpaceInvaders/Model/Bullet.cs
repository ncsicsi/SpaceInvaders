using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public struct Bullet
    {
        private int _x;
        private int _y;
        private bool _alive;
        private bool _isNewBullet;
        private int _speed;
        private int _hight;
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
        public bool Alive { get { return _alive; } set { _alive = value; } }
        public bool IsNewBullet { get { return _isNewBullet; } set { _isNewBullet = value; } }
        public int Speed { get { return _speed; } set { _speed = value; } }
        public int Hight { get { return _hight; } set { _hight = value; } }
    }
}

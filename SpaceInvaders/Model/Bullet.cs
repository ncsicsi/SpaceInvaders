using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public struct Bullet
    {
        private int _bulletX;
        private int _bulletY;
        //private bool _bullet;
        private bool _bulletAlive;
        public int X { get { return _bulletX; } set { _bulletX = value; } }
        public int Y { get { return _bulletY; } set { _bulletY = value; } }
        public bool Alive { get { return _bulletAlive; } set { _bulletAlive = value; } }
    }
}

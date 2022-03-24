using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public struct EnemyStruct
    {
        bool _alive;
        int _x;
        int _y;
        int _type;   // 1-3 tipus
        public void Alive(bool alive) { _alive = alive; }
        public bool Alive() { return _alive; }
        public void Type(int type) { _type = type; }
        public int Type() { return _type; }

        public void X(int x) { _x = x; }
        public void Y(int y) { _y = y; }
        public int X() { return _x; }
        public int Y() { return _y; }
    }
}

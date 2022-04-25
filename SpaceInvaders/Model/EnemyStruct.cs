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
        double _x;
        int _y;
        enyemyType _type;   // 1-3 tipus
        bool _isMostLeft;
        bool _isMostRight;
        bool _isMostDown;
        public enum enyemyType { RED, ORANGE, BLUE };

        public bool Alive { get { return _alive; } set { _alive = value; } }
        public enyemyType Type { get { return _type; } set { _type = value; } }


        public void X(double x) { _x = x; }
        public void Y(int y) { _y = y; }
        public double X() { return _x; }
        public int Y() { return _y; }


    }
}

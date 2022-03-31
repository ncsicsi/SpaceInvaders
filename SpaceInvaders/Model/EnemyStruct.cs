﻿using System;
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
        enyemyType _type;   // 1-3 tipus
        bool _isMostLeft;
        bool _isMostRight;
        bool _isMostDown;
        public enum enyemyType { RED, ORANGE, BLUE };

        public void Alive(bool alive) { _alive = alive; }
        public bool Alive() { return _alive; }
        public enyemyType Type { get { return _type; } set { _type = value; } }


        public void X(int x) { _x = x; }
        public void Y(int y) { _y = y; }
        public int X() { return _x; }
        public int Y() { return _y; }


    }
}

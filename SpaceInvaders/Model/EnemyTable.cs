using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    internal class EnemyTable
    {
        #region Fields
        private EnemyStruct[] _enemys;
        private static int _maxCount = 50;
        private static int _size = 45;
        private static int _columns = 10;
        private static int _rows = 5;
        private static int _distance = 10;
        private int _count = 50;
        private double _basicSpeed = 1;
        private int _bulletTimeDistance = 300;   //milyen idokozonkent lonek az enemyk 300
        private Bullet _bullet;
        private double _speed;
        private int _buttomYPos;   //legalso enemy helyzete x szerint
        private (int, int) _mostRightEnemySerial;
        private (int, int) _mostLeftEnemySerial;
        private (int, int) _mostButtomEnemySerial;


        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model;

namespace SpaceInvaders.Model
{
    public class EnemyEventArgs : EventArgs
    {
        private  EnemyStruct[,] _enemys;
        private Bullet[] _bullets;
        private int _enemyColumns;
        private int _enemyRows;
        private int _enemySize;
        private int _enemyCount;
        private int _bulletCount;
        public int EnemyColumns { get { return _enemyColumns; } set { _enemyColumns = value; } }
        public int EnemRows { get { return _enemyRows; } set { _enemyRows = value; } }
        public int EnemySize { get { return _enemySize; } set { _enemySize = value; } }
        public int EnemyCount { get { return _enemyCount; } set { _enemyCount = value; } }
        public int BulletCount { get { return _bulletCount; } set { _bulletCount = value; } }
        public EnemyStruct[,] Enemys { get { return _enemys; } set { _enemys = value; } }
        public Bullet[] Bullets { get { return _bullets; } set { _bullets = value; } }
        public EnemyEventArgs(EnemyStruct[,] enemys, int enemyColumns, int enemyRows, int enemySize, int enemyCount, int bulletCount, Bullet[] bullets)
        {
            _enemys = enemys;
            _enemyColumns = enemyColumns;
            _enemyRows = enemyRows;
            _enemySize = enemySize;
            _enemyCount = enemyCount;
            _bulletCount = bulletCount;
            _bullets = bullets;
        }

    }
}

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
        private  EnemyStruct[,] _enemys = new EnemyStruct[5,10];
        private int _enemyColumns;
        private int _enemyRows;
        private int _enemySize;
        private int _enemyCount;
        public int EnemyColumns { get { return _enemyColumns; } set { _enemyColumns = value; } }
        public int EnemRows { get { return _enemyRows; } set { _enemyRows = value; } }
        public int EnemySize { get { return _enemySize; } set { _enemySize = value; } }
        public int EnemyCount { get { return _enemyCount; } set { _enemyCount = value; } }
        public EnemyStruct[,] Enemys { get { return _enemys; } set { _enemys = value; } }
        public EnemyEventArgs(EnemyStruct[,] enemys, int enemyColumns, int enemyRows, int enemySize, int enemyCount)
        {
            _enemys = enemys;
            _enemyColumns = enemyColumns;
            _enemyRows = enemyRows;
            _enemySize = enemySize;
            _enemyCount = enemyCount;
        }

    }
}

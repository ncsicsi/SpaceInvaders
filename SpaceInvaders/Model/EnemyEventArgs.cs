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
        public int EnemyColumns { get { return _enemyColumns; } set { _enemyColumns = value; } }
        public int EnemRows { get { return _enemyRows; } set { _enemyRows = value; } }
        public int EnemySize { get { return _enemySize; } set { _enemySize = value; } }
        public EnemyStruct[,] Enemys { get { return _enemys; } set { _enemys = value; } }
        public EnemyEventArgs(EnemyStruct[,] enemys, int enemyColumns, int enemyRows, int enemySize)
        {
            _enemys = enemys;
            _enemyColumns = enemyColumns;
            _enemyRows = enemyRows;
            _enemySize = enemySize;
        }

    }
}

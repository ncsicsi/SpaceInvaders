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
        public EnemyStruct[,] Enemys { get { return _enemys; } set { _enemys = value; } }
        public EnemyEventArgs(EnemyStruct[,] enemys)
        {
            _enemys = enemys;
        }

    }
}

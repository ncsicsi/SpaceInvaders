using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class GameOverEventArgs : EventArgs
    {
        bool _win;
        public bool Win { get { return _win; } set { _win = value; } }
        
        public GameOverEventArgs(bool win)
        {
            _win = win;
        }

    }
}

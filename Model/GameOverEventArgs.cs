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
        bool _networkOn;
        public bool Win { get { return _win; } set { _win = value; } }
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        
        public GameOverEventArgs(bool win, bool networkOn)
        {
            _win = win;
            _networkOn = networkOn;

        }

    }
}

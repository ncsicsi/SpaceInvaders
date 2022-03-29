using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model;

namespace SpaceInvaders.Model
{
    public class NeuralNetwork
    {
        #region Fields
        bool _goLeft;
        bool _goRight;
        bool _bulletOn;
        bool _networkOn;
        public enum action { GORIGHT, GOLEFT, SHOT };
        #endregion

        #region Properties
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        #endregion

        #region Constructor
        public NeuralNetwork()
        {

        }
        #endregion

        #region Private Methods
        private void Start()
        {
            _networkOn = true;
        }

        private void Stop()
        {
            _networkOn = false;
        }
        #endregion

        #region Public methods

        public action NextAction()
        {
            Random random = new Random();
            int rd = random.Next(0,3);
            switch (rd)
            {
                case 0: 
                    return action.GORIGHT;
                    break;
                case 1: 
                    return action.GOLEFT;
                    break;
                case 2:
                    return action.SHOT;
                    break;
            }
            return action.SHOT;
        }

        #endregion

    }
}

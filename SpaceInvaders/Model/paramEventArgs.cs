using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class paramEventArgs : EventArgs
    {
        private double[] _evolutionParams;
        public double[] EvolutionParams { get { return _evolutionParams; } set { _evolutionParams = value; } }

        public paramEventArgs(double[] evolutionParams)
        {
            _evolutionParams = evolutionParams;
        }
    }
}

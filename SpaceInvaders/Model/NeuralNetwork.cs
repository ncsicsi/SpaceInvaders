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
        struct Weight
        { 
            double _value;
            int from;
            int to;
            public double Value { get { return _value; } set { _value = value; } }
        }

        #region Fields
        bool _networkOn;
        public enum action { GORIGHT, GOLEFT, SHOT };
        private double[] _hiddenNeurons;
        private double[] _incommingNeurons;
        private double[] _outcommingNeurons;
        private int _hiddenNeuronsCount;
        private int _incommingNeuronsCount = 5;
        private int _outcommingNeuronsCount = 3;
        private Weight[] _weights;


        public double _bulletDistance = 0; // enemy bullet tavolsaga kozott 0-700
        public double _enemyCount = 0;      //enemyk szama  0-50
        public double _ClosestEnemyYDistance = 0;   //lealsobb enemy_network._ClosestEnemyDirection = 0; tavosaga y szerint 0-700
        public double _ClosestEnemyXDistance = 0;   // legalsobb enemy tavolsaga x szerint 0-700
        public double _ClosestEnemyDirection = 0;   //jobbra vagy ballra van 0/1
        public double _enemySpeed = 0;  //enemy gyorsasaga
        public double _enemyMoveDirection = 0; //enemy mozgas iranya
        #endregion

        #region Properties
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        #endregion

        #region Constructor
        public NeuralNetwork(int hiddenNeuronsCount)
        {
            _hiddenNeuronsCount = hiddenNeuronsCount;
            _hiddenNeurons = new double[hiddenNeuronsCount];
            _incommingNeurons = new double[_incommingNeuronsCount]; ;
            _outcommingNeurons = new double[_outcommingNeuronsCount];
            _weights = new Weight[_incommingNeuronsCount * _hiddenNeuronsCount + _hiddenNeuronsCount * _outcommingNeuronsCount];
        }
        #endregion

        #region Private Methods
        /*private void Start()
        {
            _networkOn = true;
        }

        private void Stop()
        {
            _networkOn = false;
        }*/
        #endregion

        #region Public methods

        public action NextAction()
        {
            RefreshIncomingNeurons();
            ReSetNeurons();
            //hidden neuronok szamitasa
            for (int h = 0; h < _hiddenNeuronsCount; h++)
            {
                for (int i = 0; i < _incommingNeuronsCount; i++)
                {
                    _hiddenNeurons[h] += _incommingNeurons[i] * _weights[i * _hiddenNeuronsCount + h].Value;
                }
                _hiddenNeurons[h] =1/(1+Math.Exp(-_hiddenNeurons[h]));
            }
            //kimeneti neuronok szamitasa
            int s = _incommingNeuronsCount * _hiddenNeuronsCount;
            for (int o = 0; o < _outcommingNeuronsCount; o++)
            {
                for(int h= 0; h < _hiddenNeuronsCount; h++)
                {
                    _outcommingNeurons[o] += _hiddenNeurons[h] * _weights[s + h *_outcommingNeuronsCount + o].Value;
                }
            }
            double max = _outcommingNeurons[0];
            int maxPlace = 0;
            for (int i = 1; i < _outcommingNeuronsCount; i++)
            {
                if(_outcommingNeurons[i] > max)
                {
                    max = _outcommingNeurons[i];
                    maxPlace = i;
                }
            }


            Random random = new Random();
            int rd = random.Next(0,3);
            switch (maxPlace)
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

        private void RefreshIncomingNeurons()
        {
            _incommingNeurons[0] = _bulletDistance;
            _incommingNeurons[1] = _enemyCount;      //enemyk szama  0-50
            _incommingNeurons[2] = _ClosestEnemyYDistance;   //lealsobb enemy_network._ClosestEnemyDirection = 0; tavosaga y szerint 0-700
            _incommingNeurons[3] = _ClosestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-700
            _incommingNeurons[4] = _ClosestEnemyDirection;
        }
        private void ReSetNeurons()
        {
            for (int i = 0; i < _hiddenNeuronsCount; i++)
            {
                _hiddenNeurons[i] = 0;
            }
            for(int i= 0; i < _outcommingNeuronsCount; i++)
            {
                _outcommingNeurons[i] = 0;
            }
        }
        #endregion

    }
}

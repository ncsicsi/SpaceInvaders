﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using Extreme.Statistics.Distributions;

namespace SpaceInvaders.Model
{
    public class Network
    {
        #region Fields

        public enum evolution { SIMPLE, REDQUEEN };
        public evolution _evolutionType = evolution.SIMPLE;

        private double _incomingBiasNeuron = 1;
        private double _hiddenBiasNeuron = 1;


        private int _incommingNeuronsCount; // simanal 12, rednél 14
        private int _outcommingNeuronsCount = 3;
        private int _hiddenNeuronsCount;    //ezt a modell kuldi el a nerual neworknek, az pedig megadja a networknek a példányositáskor
        private double[] _hiddenNeurons;
        private double[] _incommingNeurons;
        private double[] _outcommingNeurons;
        private double[,] _weights;

        //evolucio
        private int _individualCount;
        private int _activeIndividual = 10;
        private int _bestIndividual = 0;
        private int _worstIndividual = 9;
        private int _rdIndividual = 0;
        private double[] _individualFittnes;
        private int[] _individualScore;
        private int _roundCounter;
        private double _learningTime = 0;

        //bejovo neuronok
        public double _bulletDistance = 0; // enemy bullet tavolsaga kozott 0-700
        public double _bulletXRightDistance = 0; //jobbra a bullet tavolsga (0, ha nincs bullet/balra van)
        public double _bulletXLeftDistance = 0; //balra a bullet tavolsga (0, ha nincs bullet/jobbraa van)
        public double _enemyCount = 0;      //enemyk szama  0-50
        public double _closestEnemyYDistance = 0;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-700
        public double _closestEnemyXDistance = 0;   // legalsobb enemy tavolsaga x szerint 0-700
        public double _closestEnemyDirection = 0;   //jobbra vagy ballra van 0/1
        public double _lives = 0;   //eletpontok szama
        public double _leftDistanc; //hajo mennnyit tud meg balra menni
        public double _rightDistanc; //hajo mennnyit tud meg balra menni
        public double _rightEnemyCount; // jobbra levo enemyk szama
        public double _leftEnemyCount;  // balra levo enemyk szama
        public double _enemySpeed = 0;  //enemy gyorsasaga
        public double _enemyMoveDirection = 0; //enemy mozgas iranya

        public enum action { GORIGHT, GOLEFT, SHOT };

        #endregion



        #region Constructor
        public Network(evolution type, int individualCount, int hiddenNeuronsCount)
        {
            _hiddenNeuronsCount = hiddenNeuronsCount;
            _hiddenNeurons = new double[hiddenNeuronsCount];
            if (type == evolution.SIMPLE)
            {
                _incommingNeuronsCount = 12;
            }
            else
            {
                _incommingNeuronsCount = 14;
            }
            _incommingNeurons = new double[_incommingNeuronsCount + 1];
            _outcommingNeurons = new double[_outcommingNeuronsCount];
            _individualCount = individualCount;
            _individualFittnes = new double[_individualCount];
            _individualScore = new int[_individualCount];
            _roundCounter = 0;
            _weights = new double[_individualCount, (_incommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount];
            for (int i = 0; i < _individualCount; i++)
            {
                _activeIndividual = i;
                ResetNetrowkWeights();
            }
            _evolutionType = type;
            _activeIndividual = 0;
        }

        #endregion

        #region Public Methods

        public action NextAction()
        {
            RefreshIncomingNeurons();
            ReSetNeurons();
            //hidden neuronok szamitasa
            if (_evolutionType == Network.evolution.SIMPLE)
            {
                for (int h = 0; h < _hiddenNeuronsCount; h++)
                {
                    for (int i = 0; i < _incommingNeuronsCount; i++)
                    {
                        _hiddenNeurons[h] += _incommingNeurons[i] * _weights[_activeIndividual, i * _hiddenNeuronsCount + h];
                    }
                    _hiddenNeurons[h] = 1 / (1 + Math.Exp(-_hiddenNeurons[h]));
                }
                //kimeneti neuronok szamitasa
                int s = _incommingNeuronsCount * _hiddenNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    _outcommingNeurons[o] += _hiddenBiasNeuron * _weights[_activeIndividual, s + o];
                }
                s = _incommingNeuronsCount * _hiddenNeuronsCount + _outcommingNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    for (int h = 0; h < _hiddenNeuronsCount; h++)
                    {
                        _outcommingNeurons[o] += _hiddenNeurons[h] * _weights[_activeIndividual, s + h * _outcommingNeuronsCount + o];
                    }
                }
            }

            action returnAction;

            returnAction = MaxResult();
            //returnAction = RandomMaxResult();
            //returnAction = SoftMaxRsul();

            return returnAction;
        }

        #endregion

        #region Private Methods

        //Kiertekeo fuggvenyek
        private action MaxResult()
        {
            double max = _outcommingNeurons[0];
            int maxPlace = 0;
            for (int i = 1; i < _outcommingNeuronsCount; i++)
            {
                if (_outcommingNeurons[i] > max)
                {
                    max = _outcommingNeurons[i];
                    maxPlace = i;
                }
            }

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

        private action RandomMaxResult()
        {

            Random random = new Random();
            double rd = random.Next(1, 1000000000);
            rd = rd / 1000000000D;


            double sum = 0;
            for (int o = 0; o < _outcommingNeuronsCount; o++)
            {
                sum += _outcommingNeurons[o];
            }

            double intervallMin = 0;
            double intervallMax = 0;
            int interval = 0;

            for (int o = 0; o < _outcommingNeuronsCount; o++)
            {
                intervallMin = intervallMax;
                intervallMax += (_outcommingNeurons[o] / sum);
                if (rd >= intervallMin && rd < intervallMax)
                {
                    interval = o;
                }
            }

            switch (interval)
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

        private action SoftMaxRsul()
        {
            Random random = new Random();
            double rd = random.Next(1, 1000000000);
            rd = rd / 1000000000D;

            double intervallMin = 0;
            double intervallMax = 0;
            int interval = 0;

            double sum = 0;
            double[] p = new double[_outcommingNeuronsCount];
            for (int i = 0; i < _outcommingNeuronsCount; i++)
            {
                sum += Math.Exp(_outcommingNeurons[i]);
            }

            for (int o = 0; o < _outcommingNeuronsCount; o++)
            {
                intervallMin = intervallMax;
                intervallMax += (Math.Exp(_outcommingNeurons[o]) / sum);
                p[o] = (Math.Exp(_outcommingNeurons[o]) / sum);
                if (rd >= intervallMin && rd < intervallMax)
                {
                    interval = o;
                }
            }

            switch (interval)
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
            _incommingNeurons[0] = _incomingBiasNeuron;
            _incommingNeurons[1] = _bulletDistance;
            _incommingNeurons[2] = _bulletXRightDistance;
            _incommingNeurons[3] = _bulletXLeftDistance;
            _incommingNeurons[4] = _enemyCount;      //enemyk szama  0-50
            _incommingNeurons[5] = _closestEnemyYDistance;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-70
            _incommingNeurons[6] = _closestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-70
            _incommingNeurons[7] = _closestEnemyDirection;   //jobbra vagy balra van az enemy
            _incommingNeurons[8] = _lives;  //eletpontok szama
            _incommingNeurons[9] = _leftDistanc;  //hajo mennnyit tud meg balra menni
            _incommingNeurons[10] = _rightDistanc;  //hajo mennnyit tud meg jobbra menni
            _incommingNeurons[11] = _rightEnemyCount;    //jobbra levo enemyk szama
            _incommingNeurons[12] = _leftEnemyCount;    //balra levo enemyk szama
            if (_evolutionType == Network.evolution.REDQUEEN)
            {
                _incommingNeurons[13] = _enemySpeed;    // Ellenseg gyorsasaga
                _incommingNeurons[14] = _enemyMoveDirection;    // Ellenseg mozgasanak iranya
            }
        }

        private void ReSetNeurons()
        {
            for (int i = 0; i < _hiddenNeuronsCount; i++)
            {
                _hiddenNeurons[i] = 0;
            }
            for (int i = 0; i < _outcommingNeuronsCount; i++)
            {
                _outcommingNeurons[i] = 0;
            }
        }

        private void ResetNetrowkWeights()
        {

            int s = (_incommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount;
            for (int i = 0; i < s; i++)
            {
                double varienece = 2.0D / (_incommingNeuronsCount + _hiddenNeuronsCount + 2);
                double stddev = Math.Sqrt(varienece);
                var random = new Random();
                double sample = Math.Abs(NormalDistribution.Sample(random, 0D, stddev));
                _weights[_activeIndividual, i] = sample;
            }


        }

        private void RoundResults()
        {
            double maxFittnes = _individualFittnes[0];
            _bestIndividual = 0;
            for (int i = 1; i < _individualCount; i++)
            {
                if (_individualFittnes[i] > maxFittnes)
                {
                    _bestIndividual = i;
                    maxFittnes = _individualFittnes[i];
                }
            }
            double minFittnes = _individualFittnes[_individualCount - 1];
            _worstIndividual = _individualCount - 1;
            for (int i = _individualCount - 2; i > -1; i--)
            {
                if (_individualFittnes[i] < minFittnes)
                {
                    _worstIndividual = i;
                    minFittnes = _individualFittnes[i];
                }
            }
        }

        #endregion
    }
}

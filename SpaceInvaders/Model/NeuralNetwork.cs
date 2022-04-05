﻿using System;
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
        bool _networkOn;
        //network
        public enum action { GORIGHT, GOLEFT, SHOT };
        private double[] _hiddenNeurons;
        private double[] _incommingNeurons;
        private double[] _outcommingNeurons;
        private int _hiddenNeuronsCount;
        private int _incommingNeuronsCount = 6;
        private int _outcommingNeuronsCount = 3;
        private double[,] _weights;
        //evolucio
        private int _individualCount;
        private int _activeIndividual = 10;
        private int _bestIndividual = 0;
        private int _worstIndividual = 9;
        private int[] _indicidualScores;

        public double _bulletDistance = 0; // enemy bullet tavolsaga kozott 0-700
        public double _enemyCount = 0;      //enemyk szama  0-50
        public double _closestEnemyYDistance = 0;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-700
        public double _closestEnemyXDistance = 0;   // legalsobb enemy tavolsaga x szerint 0-700
        public double _closestEnemyDirection = 0;   //jobbra vagy ballra van 0/1
        public double _lives = 0;   //eletpontok szama
        public double _enemySpeed = 0;  //enemy gyorsasaga
        public double _enemyMoveDirection = 0; //enemy mozgas iranya
        #endregion

        #region Properties
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        #endregion

        #region Constructor
        public NeuralNetwork(int hiddenNeuronsCount, int individualCount)
        {
            CreatePopulation(hiddenNeuronsCount, individualCount);
        }
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
                    _hiddenNeurons[h] += _incommingNeurons[i] * _weights[_activeIndividual, i * _hiddenNeuronsCount + h];
                }
                _hiddenNeurons[h] =1/(1+ Math.Exp(-_hiddenNeurons[h]));
            }
            //kimeneti neuronok szamitasa
            int s = _incommingNeuronsCount * _hiddenNeuronsCount;
            for (int o = 0; o < _outcommingNeuronsCount; o++)
            {
                for(int h= 0; h < _hiddenNeuronsCount; h++)
                {
                    _outcommingNeurons[o] += _hiddenNeurons[h] * _weights[_activeIndividual, s + h *_outcommingNeuronsCount + o];
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

        #endregion

        #region Network Private Methods 

        private void RefreshIncomingNeurons()
        {
            _incommingNeurons[0] = _bulletDistance;
            _incommingNeurons[1] = _enemyCount;      //enemyk szama  0-50
            _incommingNeurons[2] = _closestEnemyYDistance;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-70
            _incommingNeurons[3] = _closestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-70
            _incommingNeurons[4] = _closestEnemyDirection;   //jobbra vagy balra van az enemy
            _incommingNeurons[5] = _lives;  //eletpontok szama
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
        //sulyok elsonek legyenek random szamok
        private void ResetNetrowkWeights()
        {
            int s = _incommingNeuronsCount * _hiddenNeuronsCount + _hiddenNeuronsCount * _outcommingNeuronsCount;
            for (int i = 0; i < s; i++)
            {
                Random random = new Random();
                double rd = random.Next(0, 10);
                _weights[_activeIndividual, i] = rd/10D;
            }
        }
        #endregion

        # region Evolution Private Methods

        private void CreatePopulation(int hiddenNeuronsCount, int individualCount)
        {
            _hiddenNeuronsCount = hiddenNeuronsCount;
            _hiddenNeurons = new double[hiddenNeuronsCount];
            _incommingNeurons = new double[_incommingNeuronsCount]; ;
            _outcommingNeurons = new double[_outcommingNeuronsCount];
            _individualCount = individualCount;
            _indicidualScores = new int[_individualCount];
            _weights = new double[_individualCount, _incommingNeuronsCount * _hiddenNeuronsCount + _hiddenNeuronsCount * _outcommingNeuronsCount];
            for(int i=0; i< _individualCount; i++)
            {
                _activeIndividual = i;
                ResetNetrowkWeights();
            }
            _activeIndividual = 0;
        }

        private void RoundResults()
        {
            int maxScore = _indicidualScores[0];
            _bestIndividual = 0;
            for(int i = 1; i<_individualCount; i++)
            {
                if (_indicidualScores[i] > maxScore)
                {
                    _bestIndividual = i;
                    maxScore = _indicidualScores[i];
                }
            }            
            int minScore = _indicidualScores[9];
            _bestIndividual = 9;
            for(int i = 8; i > 0; i--)
            {
                if (_indicidualScores[i] < minScore)
                {
                    _worstIndividual = i;
                    minScore = _indicidualScores[i];
                }
            }
        }
        #endregion

        #region Evolution Public Methods

        public void GameOver(int score, bool win)
        {
            if (!win)
            {
                _indicidualScores[_activeIndividual] = score;
                if (_activeIndividual < _individualCount-1)
                {
                    _activeIndividual++;
                }
                else
                {
                    _activeIndividual = 0;
                    RoundResults();
                }
            }
        }
        #endregion

    }
}

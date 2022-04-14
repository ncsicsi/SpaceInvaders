using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using Extreme.Statistics.Distributions;
using SpaceInvaders.Model;
using SpaceInvaders.Persistence;

namespace SpaceInvaders.Model
{
    public class NeuralNetwork
    {
        #region Fields
        private IGameDataAccess _dataAccess; //adateleres
        //network
        public bool _networkOn;
        public enum action { GORIGHT, GOLEFT, SHOT };
        private double[] _hiddenNeurons;
        private double[] _incommingNeurons;
        private double[] _outcommingNeurons;
        private int _hiddenNeuronsCount;
        private int _incommingNeuronsCount = 11;
        private int _outcommingNeuronsCount = 3;
        private double[,] _weights;


        //evolucio
        private int _individualCount;
        private int _activeIndividual = 10;
        private int _bestIndividual = 0;
        private int _worstIndividual = 9;
        private int _rdIndividual = 0;
        private int[] _indicidualScores;
        private int _roundCounter;

        //bejovo neuronok
        public double _bulletDistance = 0; // enemy bullet tavolsaga kozott 0-700
        public double _bulletXRightDistance = 0; //jobbra a bullet tavolsga (0, ha nincs bullet/balra van)
        public double _bulletXLeftDistance = 0; //balra a bullet tavolsga (0, ha nincs bullet/jobbraa van)
        public double _enemyCount = 0;      //enemyk szama  0-50
        public double _closestEnemyYDistance = 0;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-700
        public double _closestEnemyXDistance = 0;   // legalsobb enemy tavolsaga x szerint 0-700
        public double _closestEnemyDirection = 0;   //jobbra vagy ballra van 0/1
        public double _lives = 0;   //eletpontok szama
        public double _xPos; //hajo pozicioja x tengely szeirnt
        public double _rightEnemyCount; // jobbra levo enemyk szama
        public double _leftEnemyCount;  // balra levo enemyk szama
        public double _enemySpeed = 0;  //enemy gyorsasaga
        public double _enemyMoveDirection = 0; //enemy mozgas iranya
        #endregion

        #region Properties
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        public double[,] Weights { get {return _weights; } }
        public int WeightsCount { get {return _incommingNeuronsCount * _hiddenNeuronsCount + _hiddenNeuronsCount * _outcommingNeuronsCount; } }
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
                if (_hiddenNeurons[h] == 1)
                {
                    int bug = 1;
                }
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

        public void LoadNetwork(Data data)
        {
            //_hiddenNeuronsCount = data._weightsSize;
            _individualCount = data._populationSize;
            for(int i = 0; i < _individualCount; i++)
            {
                for(int j=0; j< data._weightsSize; j++)
                {
                    _weights[i,j] = data._weights[i, j];
                }
            }
        }

        #endregion

        #region Network Private Methods 

        private void RefreshIncomingNeurons()
        {
            _incommingNeurons[0] = _bulletDistance;
            _incommingNeurons[1] = _bulletXRightDistance;
            _incommingNeurons[2] = _bulletXLeftDistance;
            _incommingNeurons[3] = _enemyCount;      //enemyk szama  0-50
            _incommingNeurons[4] = _closestEnemyYDistance;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-70
            _incommingNeurons[5] = _closestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-70
            _incommingNeurons[6] = _closestEnemyDirection;   //jobbra vagy balra van az enemy
            _incommingNeurons[7] = _lives;  //eletpontok szama
            _incommingNeurons[8] = _xPos;  //hajo pozicioja x tengely szerint
            _incommingNeurons[9] = _rightEnemyCount;    //jobbra levo enemyk szama
            _incommingNeurons[10] = _leftEnemyCount;    //balra levo enemyk szama


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
                double varienece = 2.0D / (_incommingNeuronsCount + _hiddenNeuronsCount);
                double stddev = Math.Sqrt(varienece);
                var random = new Random();
                double sample = Math.Abs( NormalDistribution.Sample(random, 0D, stddev));
                _weights[_activeIndividual, i] = sample;
                //NormalDistribution(5, stddev).
                //sample = NormalDistribution.Sample(5.0, stddev);
                /*Random random = new Random();
                double rd = random.Next(0, 5000);
                _weights[_activeIndividual, i] = rd / 1000000D;*/
            }
        }
        private static double SampleGaussian(Random random, double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
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
            _roundCounter = 0;
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
            int minScore = _indicidualScores[_individualCount-1];
            _worstIndividual = _individualCount-1;
            for(int i = _individualCount-2; i > 0; i--)
            {
                if (_indicidualScores[i] < minScore)
                {
                    _worstIndividual = i;
                    minScore = _indicidualScores[i];
                }
            }
        }

        private void EvolutePopulation()
        {
            Random random = new Random();
            double mutation;
            int rd = random.Next(0, 10);
            Random mutationRd = new Random();
            while ( rd != _bestIndividual && rd != _worstIndividual)
            {
                rd = random.Next(0, 10);
            }
            _rdIndividual = rd; 
            for (int i=0; i < _incommingNeuronsCount * _hiddenNeuronsCount + _hiddenNeuronsCount * _outcommingNeuronsCount; i++)
            {
                /*rd = random.Next(1,10);
                mutation = random.Next(0,1000);
                mutation = mutation / 1000000D;*/
                mutation = NormalDistribution.Sample(mutationRd, 0D, 0.5D);
                if (rd < 6)    // legjobbtol kapja a gent
                {
                    _weights[_worstIndividual, i] = _weights[_bestIndividual, i] + mutation;
                }
                else
                {
                    //randomtol kapja
                    _weights[_worstIndividual, i] = _weights[_rdIndividual, i] - mutation;
                }
            } ;
        }
        #endregion

        #region Evolution Public Methods

        public void GameOver(int score, bool win)
        {
            if (!win)
            {
                _indicidualScores[_activeIndividual] = score;
                if (_activeIndividual < _individualCount-1 && _roundCounter < _individualCount - 1)
                {
                    _activeIndividual++;
                    _roundCounter++;
                }
                else
                {
                    RoundResults();
                    EvolutePopulation();
                    _activeIndividual = _worstIndividual;
                }
            }
        }
        #endregion

    }
}

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
        //private IGameDataAccess _dataAccess; //adateleres
        //network
        public enum evolution { SIMPLE, REDQUEEN};
        public evolution _evolutionType = evolution.SIMPLE;
        public bool _networkOn;

        private double _incomingBiasNeuron = 1;
        private double _hiddenBiasNeuron = 1;
        private int _incommingNeuronsCount = 12;
        private int _simpleIncommingNeuronsCount = 12;
        private int _redQueenIncommingNeuronsCount = 14;
        private int _outcommingNeuronsCount = 3;
        private int _hiddenNeuronsCount;
        private double[] _hiddenNeurons;
        private double[] _simpleIncommingNeurons;
        private double[] _redQueenIncommingNeurons;
        private double[] _outcommingNeurons;
        private double[,] _simpleWeights;
        private double[,] _redQueenWeights;
        public enum action { GORIGHT, GOLEFT, SHOT };

        //evolucio
        private int _individualCount;
        private int _activeIndividual = 10;
        private int _bestIndividual = 0;
        private int _worstIndividual = 9;
        private int _rdIndividual = 0;
        private double[] _individualFittnes;
        private int[] _individualScore;
        private int _simpleRoundCounter;
        private int _redQueenRoundCounter;
        private double _learningTime = 0;
        private double _simpleLearningTime = 0;
        private double _redQueenLearningTime = 0;

        //fittnes
        private double _score = 0;
        private double _elapsedTime = 0;
        private double _avoidBullets = 0;
        private double _usedBullets = 0;
        //fittnes sulyok
        private double _scoreWeight = 2;
        private double _elapsedTimeWeighht = 2;
        private double _avoidBulletsWeight = 15; 
        private double _usedBulletssWeight = 0.8;



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
        #endregion

        #region Properties
        public bool NetworkOn { get { return _networkOn; } set { _networkOn = value; } }
        public double[,] Weights { get { if (_evolutionType == evolution.SIMPLE) { return _simpleWeights; } else return _redQueenWeights; } }
        public double[] IndividualFittnes { get {return _individualFittnes; } }
        public int WeightsCount { get {return (_incommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount; } }
        public double Score { get { return _score; } set { _score = value; } }
        public double ElapsedTime { get { return _elapsedTime; } set { _elapsedTime = value; } }
        public double AvoidBullets { get { return _avoidBullets; } set { _avoidBullets = value; } }
        public double UsedBullets { get { return _usedBullets; } set { _usedBullets = value; } }
        public int ActiveIndividual { get { return _activeIndividual; } set { _activeIndividual = value; } }
        public double LearningTime { get { return _learningTime; } set { _learningTime = value; } }
        public int[] IndividualScore { get { return _individualScore; } }
        public evolution EvolutionType { get { return _evolutionType; } set { _evolutionType = value; } }
        public int Round { get { if (_evolutionType == evolution.SIMPLE) { return _simpleRoundCounter; } else return _redQueenRoundCounter; } set { if (_evolutionType == evolution.SIMPLE) { _simpleRoundCounter = value;  } else _redQueenRoundCounter = value; } }
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
            if (_evolutionType == evolution.SIMPLE)
            {
                for (int h = 0; h < _hiddenNeuronsCount; h++)
                {
                    for (int i = 0; i < _incommingNeuronsCount; i++)
                    {
                        _hiddenNeurons[h] += _simpleIncommingNeurons[i] * _simpleWeights[_activeIndividual, i * _hiddenNeuronsCount + h];
                    }
                    _hiddenNeurons[h] = 1 / (1 + Math.Exp(-_hiddenNeurons[h]));
                    if (_hiddenNeurons[h] == 1)
                    {
                        int bug = 1;
                    }
                }
                //kimeneti neuronok szamitasa
                int s = _incommingNeuronsCount * _hiddenNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    _outcommingNeurons[o] += _hiddenBiasNeuron * _simpleWeights[_activeIndividual, s + o];
                }
                s = _incommingNeuronsCount * _hiddenNeuronsCount + _outcommingNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    for (int h = 0; h < _hiddenNeuronsCount; h++)
                    {
                        _outcommingNeurons[o] += _hiddenNeurons[h] * _simpleWeights[_activeIndividual, s + h * _outcommingNeuronsCount + o];
                    }
                }
            }
            else
            {
                for (int h = 0; h < _hiddenNeuronsCount; h++)
                {
                    for (int i = 0; i < _incommingNeuronsCount; i++)
                    {
                        _hiddenNeurons[h] += _redQueenIncommingNeurons[i] * _redQueenWeights[_activeIndividual, i * _hiddenNeuronsCount + h];
                    }
                    _hiddenNeurons[h] = 1 / (1 + Math.Exp(-_hiddenNeurons[h]));
                    if (_hiddenNeurons[h] == 1)
                    {
                        int bug = 1;
                    }
                }
                //kimeneti neuronok szamitasa
                int s = _incommingNeuronsCount * _hiddenNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    _outcommingNeurons[o] += _hiddenBiasNeuron * _redQueenWeights[_activeIndividual, s + o];
                }
                s = _incommingNeuronsCount * _hiddenNeuronsCount + _outcommingNeuronsCount;
                for (int o = 0; o < _outcommingNeuronsCount; o++)
                {
                    for (int h = 0; h < _hiddenNeuronsCount; h++)
                    {
                        _outcommingNeurons[o] += _hiddenNeurons[h] * _redQueenWeights[_activeIndividual, s + h * _outcommingNeuronsCount + o];
                    }
                }
            }
            action returnAction;
            
            returnAction = MaxResult();
            //returnAction = RandomMaxResult();
            //returnAction = SoftMaxRsul();

            return returnAction;
        }

        public void LoadNetwork(Data data)
        {
            //_hiddenNeuronsCount = data._weightsSize;
            _individualCount = data._populationSize;
            if (data._evolutionType == 0)
            {
                _evolutionType = evolution.SIMPLE;
                _incommingNeuronsCount = _simpleIncommingNeuronsCount;
                _simpleRoundCounter = data._round;
                for (int i = 0; i < _individualCount; i++)
                {
                    for (int j = 0; j < data._weightsSize; j++)
                    {
                        _simpleWeights[i, j] = data._weights[i, j];
                    }
                    _individualFittnes[i] = data._individualFittnes[i];
                }
            }
            else
            {
                _evolutionType = evolution.REDQUEEN;
                _incommingNeuronsCount = _redQueenIncommingNeuronsCount;
                _redQueenRoundCounter = data._round;
                for (int i = 0; i < _individualCount; i++)
                {
                    for (int j = 0; j < data._weightsSize; j++)
                    {
                        _redQueenWeights[i, j] = data._weights[i, j];
                    }
                    _individualFittnes[i] = data._individualFittnes[i];
                }
            }
            RoundResults();
            _activeIndividual = _worstIndividual;
            
            ReSetFittnes();
            _learningTime = data._learningTime;
            _individualScore = data._individualScore;
        }
        public void BestPlay()
        {
            _activeIndividual = _bestIndividual;
            ReSetFittnes();
        }

        public void TurnSimpleEvolution()
        {
            EvolutionType = NeuralNetwork.evolution.SIMPLE;
            _redQueenLearningTime = _learningTime;
            LearningTime = _simpleLearningTime;
            ActiveIndividual = 0;
            _incommingNeuronsCount = _simpleIncommingNeuronsCount;
            ReSetFittnes();
        }        
        public void TurnRedQueenEvolution()
        {
            EvolutionType = NeuralNetwork.evolution.REDQUEEN;
            _simpleLearningTime = _learningTime;
            LearningTime = _redQueenLearningTime;
            ActiveIndividual = 0;
            _incommingNeuronsCount = _redQueenIncommingNeuronsCount;
            ReSetFittnes();
        }

        #endregion

        #region Network Private Methods 
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
            double []p =new double [_outcommingNeuronsCount];
            for(int i=0; i < _outcommingNeuronsCount; i++)
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
            if (_evolutionType == evolution.SIMPLE)
            {
                _simpleIncommingNeurons[0] = _incomingBiasNeuron;
                _simpleIncommingNeurons[1] = _bulletDistance;
                _simpleIncommingNeurons[2] = _bulletXRightDistance;
                _simpleIncommingNeurons[3] = _bulletXLeftDistance;
                _simpleIncommingNeurons[4] = _enemyCount;      //enemyk szama  0-50
                _simpleIncommingNeurons[5] = _closestEnemyYDistance;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-70
                _simpleIncommingNeurons[6] = _closestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-70
                _simpleIncommingNeurons[7] = _closestEnemyDirection;   //jobbra vagy balra van az enemy
                _simpleIncommingNeurons[8] = _lives;  //eletpontok szama
                _simpleIncommingNeurons[9] = _leftDistanc;  //hajo mennnyit tud meg balra menni
                _simpleIncommingNeurons[10] = _rightDistanc;  //hajo mennnyit tud meg jobbra menni
                _simpleIncommingNeurons[11] = _rightEnemyCount;    //jobbra levo enemyk szama
                _simpleIncommingNeurons[12] = _leftEnemyCount;    //balra levo enemyk szama
            }
            else
            {
                _redQueenIncommingNeurons[0] = _incomingBiasNeuron;
                _redQueenIncommingNeurons[1] = _bulletDistance;
                _redQueenIncommingNeurons[2] = _bulletXRightDistance;
                _redQueenIncommingNeurons[3] = _bulletXLeftDistance;
                _redQueenIncommingNeurons[4] = _enemyCount;      //enemyk szama  0-50
                _redQueenIncommingNeurons[5] = _closestEnemyYDistance;   //lealsobb enemy_network._closestEnemyDirection = 0; tavosaga y szerint 0-70
                _redQueenIncommingNeurons[6] = _closestEnemyXDistance;   // legalsobb enemy tavolsaga x szerint 0-70
                _redQueenIncommingNeurons[7] = _closestEnemyDirection;   //jobbra vagy balra van az enemy
                _redQueenIncommingNeurons[8] = _lives;  //eletpontok szama
                _redQueenIncommingNeurons[9] = _leftDistanc;  //hajo mennnyit tud meg balra menni
                _redQueenIncommingNeurons[10] = _rightDistanc;  //hajo mennnyit tud meg jobbra menni
                _redQueenIncommingNeurons[11] = _rightEnemyCount;    //jobbra levo enemyk szama
                _redQueenIncommingNeurons[12] = _leftEnemyCount;    //balra levo enemyk szama
                _redQueenIncommingNeurons[13] = _enemySpeed;    // Ellenseg gyorsasaga
                _redQueenIncommingNeurons[14] = _enemyMoveDirection;    // Ellenseg mozgasanak iranya
            }
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
        //sulyok elsonek legyenek gauss szerint random szamok szamok
        private void ResetNetrowkWeights()
        {
            if (_evolutionType == evolution.SIMPLE)
            {
                int s = (_simpleIncommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount;
                for (int i = 0; i < s; i++)
                {
                    double varienece = 2.0D / (_simpleIncommingNeuronsCount + _hiddenNeuronsCount+2);
                    double stddev = Math.Sqrt(varienece);
                    var random = new Random();
                    double sample = Math.Abs(NormalDistribution.Sample(random, 0D, stddev));
                    _simpleWeights[_activeIndividual, i] = sample;
                }
            }
            else
            {
                int s = (_redQueenIncommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount;
                for (int i = 0; i < s; i++)
                {
                    double varienece = 2.0D / (_redQueenIncommingNeuronsCount + _hiddenNeuronsCount+2);
                    double stddev = Math.Sqrt(varienece);
                    var random = new Random();
                    double sample = Math.Abs(NormalDistribution.Sample(random, 0D, stddev));
                    _redQueenWeights[_activeIndividual, i] = sample;
                }
            }
        }

        #endregion

        # region Evolution Private Methods

        private void CreatePopulation(int hiddenNeuronsCount, int individualCount)
        {
            _hiddenNeuronsCount = hiddenNeuronsCount;
            _hiddenNeurons = new double[hiddenNeuronsCount];
            _simpleIncommingNeurons = new double[_simpleIncommingNeuronsCount + 1];
            _redQueenIncommingNeurons = new double[_redQueenIncommingNeuronsCount + 1];
            _outcommingNeurons = new double[_outcommingNeuronsCount];
            _individualCount = individualCount;
            _individualFittnes = new double[_individualCount];
            _individualScore = new int[_individualCount];
            _simpleRoundCounter = 0;
            _redQueenRoundCounter = 0;
            _simpleWeights = new double[_individualCount, (_simpleIncommingNeuronsCount + 1)* _hiddenNeuronsCount + (_hiddenNeuronsCount + 1)* _outcommingNeuronsCount];
            _redQueenWeights = new double[_individualCount, (_redQueenIncommingNeuronsCount + 1)* _hiddenNeuronsCount + (_hiddenNeuronsCount + 1)* _outcommingNeuronsCount];
            for(int i=0; i< _individualCount; i++)
            {
                _activeIndividual = i;
                ResetNetrowkWeights();
            }
            _evolutionType = evolution.REDQUEEN;
            for (int i = 0; i < _individualCount; i++)
            {
                _activeIndividual = i;
                ResetNetrowkWeights();
            }
            _evolutionType = evolution.SIMPLE;
            _activeIndividual = 0;
        }

        private void RoundResults()
        {
            double maxFittnes = _individualFittnes[0];
            _bestIndividual = 0;
            for(int i = 1; i<_individualCount; i++)
            {
                if (_individualFittnes[i] > maxFittnes)
                {
                    _bestIndividual = i;
                    maxFittnes = _individualFittnes[i];
                }
            }            
            double minFittnes = _individualFittnes[_individualCount-1];
            _worstIndividual = _individualCount-1;
            for(int i = _individualCount-2; i > -1; i--)
            {
                if (_individualFittnes[i] < minFittnes)
                {
                    _worstIndividual = i;
                    minFittnes = _individualFittnes[i];
                }
            }
        }

        private void EvolutePopulation()
        {
            if (Deadlock())
            {
                RandomIndividual();
            }
            Random random = new Random();
            double mutation;
            int rd = _bestIndividual;
            Random mutationRd = new Random();
            int weightCount = (_incommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount;
            while ( rd == _bestIndividual || rd == _worstIndividual)
            {
                rd = random.Next(0, _individualCount-1);
            }
            _rdIndividual = rd;
            if (_evolutionType == evolution.SIMPLE)
            {
                for (int i = 0; i < weightCount; i++)
                {
                    rd = random.Next(1, 100);
                    /*mutation = random.Next(0,1000);
                    mutation = mutation / 1000000D;*/
                    mutation = NormalDistribution.Sample(mutationRd, 0D, 0.02D);
                    if (rd < 60)    // legjobbtol kapja a gent
                    {
                        _simpleWeights[_worstIndividual, i] = Math.Abs(_simpleWeights[_bestIndividual, i] + mutation);
                    }
                    else
                    {
                        //randomtol kapja
                        _simpleWeights[_worstIndividual, i] = Math.Abs(_simpleWeights[_rdIndividual, i] + mutation);
                    }
                }
            }
            else
            {
                for (int i = 0; i < weightCount; i++)
                {
                    rd = random.Next(1, 10);
                    /*mutation = random.Next(0,1000);
                    mutation = mutation / 1000000D;*/
                    mutation = NormalDistribution.Sample(mutationRd, 0D, 0.02D);
                    if (rd < 6)    // legjobbtol kapja a gent
                    {
                        _redQueenWeights[_worstIndividual, i] = Math.Abs(_redQueenWeights[_bestIndividual, i] + mutation);
                    }
                    else
                    {
                        //randomtol kapja
                        _redQueenWeights[_worstIndividual, i] = Math.Abs(_redQueenWeights[_rdIndividual, i] + mutation);
                    }
                }
            }
        }
        private bool Deadlock()
        {
            bool deadlock = true;
            double fittnes = _individualFittnes[0];
            for(int i=1; i<_individualCount; i++)
            {
                if(_individualFittnes[i] != fittnes)
                {
                    deadlock = false;
                }
                else
                {
                    fittnes = _individualFittnes[i];
                }
            }
            return deadlock;

        }
        private void RandomIndividual()
        {
            Random randomIndividual = new Random();
            int rd = randomIndividual.Next(0, _individualCount);
            int s = (_incommingNeuronsCount + 1) * _hiddenNeuronsCount + (_hiddenNeuronsCount + 1) * _outcommingNeuronsCount;
            if (_evolutionType == evolution.SIMPLE)
            {
                for (int i = 0; i < s; i++)
                {
                    double varienece = 2.0D / (_incommingNeuronsCount + _hiddenNeuronsCount+2);
                    double stddev = Math.Sqrt(varienece);
                    var random = new Random();
                    double sample = Math.Abs(NormalDistribution.Sample(random, 0D, stddev));
                    _simpleWeights[rd, i] = sample;
                }
            }
            else
            {
                for (int i = 0; i < s; i++)
                {
                    double varienece = 2.0D / (_incommingNeuronsCount + _hiddenNeuronsCount+2);
                    double stddev = Math.Sqrt(varienece);
                    var random = new Random();
                    double sample = Math.Abs(NormalDistribution.Sample(random, 0D, stddev));
                    _redQueenWeights[rd, i] = sample;
                }
            }
            _activeIndividual = rd;
        }
        private void CalculateFittnes()
        {
            _individualFittnes[_activeIndividual] = _score * _scoreWeight + _elapsedTime * _elapsedTimeWeighht + _avoidBullets * _avoidBulletsWeight - _usedBullets*_usedBulletssWeight; 
        }
        private void ReSetFittnes()
        {
            _elapsedTime = 0;
            _score = 0;
            _avoidBullets = 0;
            _usedBullets = 0;
        }

        #endregion

        #region Evolution Public Methods

        public void GameOver(int score, bool win)
        {
            if (!win)
            {
                _learningTime += (_elapsedTime/3600D);
                _score = score;
                _individualScore[_activeIndividual] = score;
                CalculateFittnes();
                if (_evolutionType == evolution.SIMPLE)
                {
                    if (_activeIndividual < _individualCount - 1 && _simpleRoundCounter < _individualCount - 1)
                    {
                        _activeIndividual++;
                        ReSetFittnes();
                        _simpleRoundCounter++;
                    }
                    else
                    {
                        RoundResults();
                        _activeIndividual = _worstIndividual;
                        EvolutePopulation();
                        ReSetFittnes();
                    }
                }
                else
                {
                    if (_activeIndividual < _individualCount - 1 && _redQueenRoundCounter < _individualCount - 1)
                    {
                        _activeIndividual++;
                        ReSetFittnes();
                        _redQueenRoundCounter++;
                    }
                    else
                    {
                        RoundResults();
                        _activeIndividual = _worstIndividual;
                        EvolutePopulation();
                        ReSetFittnes();
                    }
                }
            }
        }
        #endregion

    }
}

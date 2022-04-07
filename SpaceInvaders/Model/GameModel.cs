﻿//#define RIGHT 0
//#define LEFT 1
//#define DOWN 2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Persistence;


namespace SpaceInvaders.Model
{

    internal class GameModel
    {
        #region Fields
        private NeuralNetwork _network;
        private int _populationSize = 20;
        private int _hiddenNeuronSize = 25;
        private IGameDataAccess _dataAccess; //adateleres
        private int _score;
        private int _lives;
        private int _enemysCount = 50;
        private static int _maxenemyCount=50;
        private EnemyStruct[,] _enemys;
        private static int _maxBullet = 120;
        private Bullet[] _bullets;
        private int _shipXPos;
        private int _shipYPos = 590;
        private bool _goLeft;
        private bool _goRight;
        private bool _bullet;
        private static System.Timers.Timer _timer;
        private static int _windowWidth = 700;
        private static int _windowHeight = 700;
        private static int _windowBorder = 10;
        private static int _shipWidth = 60;
        private static int _shipHeight = 30;
        private static int _shipSpeed = 5;
        private static int _enemySize = 45;
        private static int _enemyColumns = 10;
        private static int _enemyRows = 5;
        private static int _enemyDistance = 10;
        private int _bulletCount;
        private int _enemySpeed;
        private int _enemyBasicSpeed = 1;
        private int _enemyBulletTimeDistance = 300;   //milyen idokozonkent lonek az enemyk 300
        private int _enemyBulletTimeCounter;
        private Bullet _enemyBullet;
        private int _bulletHight = 20;
        private bool _win;
        private int _bulletspeed = 5;
        private int _enemyButtomYPos;
        private (int, int) _mostRightEnemySerial;
        private (int, int) _mostLeftEnemySerial;
        private (int, int) _mostButtomEnemySerial;
        private int _rounds;
        private enum direction {RIGHT, LEFT, DOWN};
        private direction _direction;
        

        #endregion

        #region Properties 
        //Pontszam lekerdezese
        public Int32 Score { get { return _score; } }
        //eletek lekerdezese
        public Int32 Lives { get { return _lives; } }
        public Int32 Rounds { get { return _rounds; } }
        //jatek vege lekerdezese
        public Boolean IsGameOver { get { return (_lives == 0); } }
        //hajo y pos lekerdezese vege lekerdezese
        public int XPos { get { return _shipXPos; } }
        // irany beallitasa
        public void GoLeft(bool goLeft) { _goLeft = goLeft; }
        public void GoRight(bool goRight) { _goRight = goRight; }
        //bullet
        public void BulletOn(bool bullet) { _bullet = bullet; }
        public bool NetworkOn { get { return _network.NetworkOn; } set { _network.NetworkOn = value; } }


        #endregion

        #region Constructor
        public GameModel(IGameDataAccess dataAccess)
        {
            _network = new NeuralNetwork(_hiddenNeuronSize,_populationSize);
            _enemys = new EnemyStruct[_enemyRows, _enemyColumns];
            _bullets = new Bullet[_maxBullet];
            ReSetBulletTable();
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _rounds = 0;
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            _timer.Stop();
            ReSetEnemyTable();
            ReSetBulletTable();
            _score = 0;
            _lives = 2;
            _shipXPos = 298;
            _bullet = false;
            _bulletCount = 0;
            _enemysCount = _maxenemyCount;
            _enemyBulletTimeCounter = 0;
            _enemyBullet.Alive = false;
            _enemyBullet.IsNewBullet = false;
            _goLeft = false;
            _goRight = false;
            _bullet = false;
            _enemySpeed = _enemyBasicSpeed;
            _direction= direction.RIGHT;
            _network.NetworkOn = true;
            _rounds++;
            OnGameCreated();
            _timer.Start();
            
        }
        public void NewRound()
        {
            _timer.Stop();
            ReSetEnemyTable();
            ReSetBulletTable();
            _shipXPos = 298;
            _bullet = false;
            _bulletCount = 0;
            _enemysCount = _maxenemyCount;
            _enemySpeed++;
            _lives++;
            _direction = direction.RIGHT;
            _goLeft = false;
            _goRight = false;
            _bullet = false;
            OnGameCreated();
            //_timer = new System.Timers.Timer(20);
            //_timer.Elapsed += _timer_Elapsed;
            //_timer.AutoReset = true;
            //_timer.Enabled = true;
            _timer.Start();
        }

        // Jatek betoltese
        public async Task LoadGameAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            Data data = await _dataAccess.LoadAsync(path);
            _rounds = data._round;
            _populationSize = data._populationSize;
            //_hiddenNeuronSize = data._weightsSize;
            _network.LoadNetwork(data);

            NetworkLoaded?.Invoke(this, new GameEventArgs(_score, _lives, _shipXPos, _bullets, _enemys, _enemyBullet));
        }

        // neuralos halok mentese mentése

        public async Task SaveNetworkAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _rounds, _populationSize, _network.WeightsCount, _network.Weights);
        }


        #endregion

        #region Events

        /// Játék létrehozásának eseménye.
        public event EventHandler<EnemyEventArgs> GameCreated;
        // Játék végének eseménye.
        public event EventHandler<GameOverEventArgs> GameOver;
        //Jatek előrehaladáskor frissitesi esemeny
        public event EventHandler<GameEventArgs> GameAdvanced;
        public event EventHandler<GameEventArgs> NetworkLoaded;
        //Enemy tabla letrehozasanak esemenye, hogy lekuldjuk a viewba


        #endregion

        #region Private Methods

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NetworkAction();
            ShipMove();
            _enemyBulletTimeCounter++;
            EnemyBulletMove();
            if (_enemyBulletTimeCounter >= _enemyBulletTimeDistance)
            {
                _enemyBulletTimeCounter = 0;
                CreateEnemyBullet();
            }
            EnemyMove();
            BulletMove();
            CreateBullet();
            OnGameAdvanced();
            if (GameOverIs())
            {
                OnGameOver(_win);
            }

        }

        private void ReSetBulletTable()
        {
            for(int i = 0; i < _maxBullet; i++)
            {
                _bullets[i].Alive = false;
                _bullets[i].IsNewBullet = false;
            }
        }

        private void ReSetEnemyTable()
        {
            int enemyColumn = 0;
            int s = _windowWidth - ((_enemyColumns * _enemySize) + ((_enemyColumns - 1) * _enemyDistance));
            int border =_windowWidth - (_windowWidth-((_enemyColumns * _enemySize) + ((_enemyColumns - 1) * _enemyDistance))) / 2 - _enemySize;
            int left = border;
            for (int i = 0; i < _enemys.Length; i++)
            {
                switch (i)
                {
                    case 10:
                        enemyColumn=0;
                        left = border;
                        break;
                    case 20:
                        enemyColumn=0;
                        left = border;
                        break;
                    case 30:
                        enemyColumn=0;
                        left = border;
                        break;
                    case 40:
                        enemyColumn=0;
                        left = border;
                        break;
                }
                EnemyStruct.enyemyType type =EnemyStruct.enyemyType.RED;
                if (i < 10) type = EnemyStruct.enyemyType.RED;
                else if(i < 30) type = EnemyStruct.enyemyType.ORANGE; 
                else if(i < 50) type = EnemyStruct.enyemyType.BLUE;

                _enemys[(i / _enemyColumns), enemyColumn].Alive = true;
                    _enemys[i / _enemyColumns, enemyColumn].Type = type;
                    _enemys[i / _enemyColumns, enemyColumn].Y(_enemyDistance+(i/ _enemyColumns) *(_enemySize+ _enemyDistance));
                    _enemys[i / _enemyColumns, enemyColumn].X(left);

                enemyColumn++;
                left -= (_enemySize + _enemyDistance);
            }
            _mostButtomEnemySerial = (_enemyRows-1, 0);
            _mostRightEnemySerial = (0,0);
            _mostLeftEnemySerial = (0, _enemyColumns-1);
            _enemyButtomYPos = _enemys[_enemyRows - 1, 0].Y() + _enemySize;
            

        }

        private void CreateBullet()
        {
            if (_bullet == true)
            {
                _bullet = false;
                _bullets[_bulletCount].X = _shipXPos + _shipWidth/2-2;
                _bullets[_bulletCount].Y = _shipYPos - _bulletHight-6;
                _bullets[_bulletCount].Alive = true;
                _bullets[_bulletCount].IsNewBullet = true;
                if (_bulletCount < _maxBullet - 1)
                {
                    _bulletCount++;
                }
                else
                {
                    _bulletCount = 0;
                }
            }
        }

        private void CreateEnemyBullet()
        {
            _enemyBullet.IsNewBullet = true;
            _enemyBullet.Alive = true;
            _enemyBullet.X = _shipXPos+(_shipWidth/2)-2;
            _enemyBullet.Y = _windowBorder + _bulletHight;
            //_enemyBullet.Y = _enemyButtomYPos-_enemySize;
        }

        private void EnemyBulletMove()
        {
            if (_enemyBullet.Alive)
            {
                _enemyBullet.IsNewBullet = false;
                _enemyBullet.Y += _bulletspeed;
                if (_enemyBullet.Y + _bulletHight >= _shipYPos && _enemyBullet.X>= _shipXPos && _enemyBullet.X <= _shipXPos  + _shipWidth )
                {
                    _lives--;
                    _enemyBullet.Alive = false;
                }else if(_enemyBullet.Y + _bulletHight >= _windowHeight){
                    _enemyBullet.Alive = false; ;
                }
            }
        }

        private void BulletMove()
        {
            for (int i = 0; i < _maxBullet; i++)
            {
                if (_bullets[i].Alive)
                {
                    _bullets[i].IsNewBullet = false;
                    _bullets[i].Y -= _bulletspeed;
                    if (_bullets[i].Y <= 5)
                    {
                        _bullets[i].Alive = false;
                    }
                    for (int j = 0; j < _enemyRows; j++)
                    {
                        for (int z = 0; z < _enemyColumns; z++)
                        {
                            if (_enemys[j, z].Alive == true && _enemys[j, z].X() <= _bullets[i].X && _enemys[j, z].X() + 45 >= _bullets[i].X && _enemys[j, z].Y() >= _bullets[i].Y - 45 && _enemys[j, z].Y() <= _bullets[i].Y)
                            {
                                _bullets[i].Alive = false;
                                _enemys[j, z].Alive = false;
                                _enemysCount--;
                                if (_enemysCount == 1)
                                {
                                    _enemySpeed++;
                                }
                                switch (_enemys[j, z].Type)
                                {
                                    case EnemyStruct.enyemyType.RED:
                                        _score += 15;
                                        break;
                                    case EnemyStruct.enyemyType.ORANGE:
                                        _score += 10;
                                        break;
                                    case EnemyStruct.enyemyType.BLUE:
                                        _score += 5;
                                        break;
                                }
                                if ((j,z)==(_mostButtomEnemySerial))
                                {
                                    NewMostDown();
                                }
                                if ((j, z) == (_mostRightEnemySerial))
                                {
                                    NewMostRight();
                                }
                                if ((j, z) == (_mostLeftEnemySerial))
                                {
                                    NewMostLeft();
                                }
                            }
                        }
                    }
                }else if (_bullets[i].Y < 0)
                {
                    _bullets[i].Alive = false;
                }
            }
        }
        private void ShipMove()
        {
            if (_goLeft && _shipXPos > _windowBorder)
            {
                _shipXPos -= _shipSpeed;
            }
            else if (_goRight && _shipXPos + _shipWidth + _windowBorder + _shipSpeed < _windowWidth)
            {
                _shipXPos += _shipSpeed;
            }
        }

        //enemy move methods
        private void EnemyMove()
        {
            if(_direction == direction.RIGHT && MostRightCoord() < 625 )
            {
                MoveRight();
            }
            else if(_direction == direction.RIGHT && MostRightCoord() >= 625)
            {
                _direction = direction.LEFT;
            }
            if(_direction == direction.LEFT && MostLeftCoord()> _windowBorder)
            {
                MoveLeft();
            }
            else if (_direction == direction.LEFT && MostLeftCoord() <= _windowBorder)
            {
                _direction = direction.DOWN;
            }
            else if(_direction == direction.DOWN)
            {
                MoveDown();
                _direction = direction.RIGHT;
            }
            _enemyButtomYPos = MostLeftDown()+_enemySize;
        }

        private int MostRightCoord()
        {
            int x; int y;
            (x, y) = _mostRightEnemySerial;
            int max = _enemys[x, y].X();
            return max;
        }        
        private int MostLeftCoord()
        {
            int x; int y;
            (x, y) = _mostLeftEnemySerial;
            int max = _enemys[x, y].X();
            return (max);
        }        
        private int MostLeftDown()
        {
            int x; int y;
            (x,y) = _mostButtomEnemySerial;
            int max = _enemys[x, y].Y()+_enemySize;
            return (max);
        }
        private void MoveLeft()
        {
            int s;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s - _enemySpeed);
                }
            }
        }
        private void MoveRight()
        {
            int s;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s + _enemySpeed);
                }
            }
        }
        private void MoveDown()
        {
            int s;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    s = _enemys[i, j].Y();
                    _enemys[i, j].Y(s + _enemySize);
                }
            }
        }
        private void NewMostRight()
        {
            int max = -1;
            int maxI = 0;
            int maxJ = 0;
            for(int i = 0; i < _enemyRows; i++)
            {
                for(int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemys[i, j].X() > max && _enemys[i,j].Alive) {
                        max = _enemys[i, j].X();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostRightEnemySerial = (maxI,maxJ);
        }
        private void NewMostLeft()
        {
            int min = 1000;
            int minI = 0;
            int minJ = 0;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemys[i, j].X() < min && _enemys[i, j].Alive)
                    {
                        min = _enemys[i, j].X();
                        minI = i;
                        minJ = j;
                    }
                }
            }
            _mostLeftEnemySerial = (minI, minJ);
        }
        private void NewMostDown()
        {
            int max = 0;
            int maxI = 0;
            int maxJ = 0;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemys[i, j].Y() > max && _enemys[i, j].Alive)
                    {
                        max = _enemys[i, j].X();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostButtomEnemySerial = (maxI, maxJ);
        }
        private void NetworkAction()
        {
            if (_network.NetworkOn)
            {
                SetNetwork();
                NeuralNetwork.action nextAction = _network.NextAction();
                switch (nextAction)
                {
                    case NeuralNetwork.action.GORIGHT:
                        _goLeft = false;
                        _goRight = true;
                        break;
                    case NeuralNetwork.action.GOLEFT:
                        _goLeft = true;
                        _goRight = false;
                        break;
                    case NeuralNetwork.action.SHOT:
                        _goLeft = false;
                        _goRight = false;
                        _bullet = true;
                        break;
                }
            }
        }
        private void SetNetwork()
        {
            if (_enemyBullet.Alive)
            {
                _network._bulletDistance = (_shipYPos - _enemyBullet.Y - _bulletHight)/10D; //enemy bullet tavolsaga
            }
            else
            {
                _network._bulletDistance = 0;
            }
            if(_enemyBullet.Alive)
            {
                if(_enemyBullet.X >_shipXPos)
                {
                    _network._bulletXRightDistance = (_enemyBullet.X - _shipXPos - _shipWidth/2)/10D;
                }
                else
                {
                    _network._bulletXRightDistance = 0;
                }
                if (_enemyBullet.X < _shipXPos)
                {
                    _network._bulletXLeftDistance =(_shipXPos - _enemyBullet.X +_shipWidth / 2)/10D;
                }
                else
                {
                    _network._bulletXLeftDistance = 0;
                }
            }
            else
            {
                _network._bulletXRightDistance = 0;
                _network._bulletXLeftDistance = 0;
            }
            _network._enemyCount = _enemysCount/1D;
            int x, y; 
            (x,y)= _mostButtomEnemySerial;
            _network._closestEnemyYDistance = (_shipYPos - _enemys[x,y].Y() - _enemySize)/10D;
            _network._closestEnemyXDistance = Math.Abs( (_shipXPos + _shipWidth/2) - (_enemys[x, y].X() + _enemySize/2))/10D;
            if(_network._closestEnemyXDistance == (_shipXPos + _shipWidth / 2) - (_enemys[x, y].X() + _enemySize / 2))
            {
                _network._closestEnemyDirection = 0;
            }
            else
            {
                _network._closestEnemyDirection = 1;
            }
            _network._lives = _lives;
            _network._xPos = _shipXPos/10D;
            _network._rightEnemyCount = RightEnemyCount();
            _network._leftEnemyCount = LeftEnemyCount();

        }
        private int RightEnemyCount()
        {
            int count = 0;
            for (int i=0; i < _enemyRows; i++) 
            {
                for(int j=0; j < _enemyColumns; j++)
                {
                    if (_enemys[i,j].Alive && _enemys[i,j].X() > _shipXPos + _shipWidth / 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private int LeftEnemyCount()
        {
            int count = 0;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemys[i, j].Alive && _enemys[i, j].X() < _shipXPos+_shipWidth/2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private bool GameOverIs()
        {
            if (_lives == 0) {
                _win = false;
                return true;
            }
            if(_enemyButtomYPos >= _shipYPos)
            {
                _win = false;
                return true;
            }
            if (_enemysCount == 0)
            {
                _win = true;
                return true;
            }
            return false;
        }


        //elorehaladasa a jateknak es frissites
        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
                GameAdvanced(this, new GameEventArgs(_score, _lives, _shipXPos, _bullets, _enemys, _enemyBullet));
        }
        //enemy tabla letrehozasanak esemenye
        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new EnemyEventArgs(_enemys, _enemyColumns, _enemyRows, _enemySize, _enemysCount, _maxBullet, _bullets));
            
        }

        //jatek vege
        private void OnGameOver(bool win)
        {
            _timer.Stop();
            _network.GameOver(_score, win);
                if (GameOver != null)
                GameOver(this, new GameOverEventArgs(win, _network.NetworkOn));
        }

        #endregion

        #region Public methods

        public void stopTimer()
        {
            _timer.Stop();
        }
        public void startTimer()
        {
            _timer.Start();
        }
        public void ChangeManual()
        {
            _goLeft = false;
            _goRight = false;
            _bullet = false;
            NetworkOn = false;
        }
        public void ChangeAI()
        {
            _goLeft = false;
            _goRight = false;
            _bullet = false;
            NetworkOn = true;
        }

        #endregion

    }
}

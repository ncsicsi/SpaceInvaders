//#define RIGHT 0
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
        //network
        private NeuralNetwork _network;
        private int _populationSize = 20;
        private int _hiddenNeuronSize = 25;
        //game param
        //private static int _maxenemyCount=50;
        private static int _maxBullet = 120;
        private static int _windowWidth = 700;
        private static int _windowHeight = 700;
        private static int _windowBorder = 10;
        //private static int _enemySize = 45;
        //private static int _enemyColumns = 10;
        //private static int _enemyRows = 5;
        //private static int _enemyDistance = 10;
        //private int _enemysCount = 50;
        //private double _enemyBasicSpeed = 1;
        //private int _enemyBulletTimeDistance = 300;   //milyen idokozonkent lonek az enemyk 300
        private int _bulletHight = 20;
        private int _bulletspeed = 5;
        //private EnemyStruct[,] _enemys;
        private EnemyTable _enemyTable;
        private Bullet[] _bullets;
        //private Bullet _enemyBullet;
        private Player _player;
        private int _bulletCount;
        //private double _enemySpeed;
        //private int _enemyBulletTimeCounter;
        private bool _win;
        //private int _enemyButtomYPos;   //legalso enemy helyzete x szerint
        //private (int, int) _mostRightEnemySerial;
        //private (int, int) _mostLeftEnemySerial;
        //private (int, int) _mostButtomEnemySerial;
        public bool _viewOn;
        private int _rounds;
        public bool _startGame = false;

        private static System.Timers.Timer _timer;
        private static System.Timers.Timer _timerOffView;

        private IGameDataAccess _dataAccess; //adateleres

        private enum direction {RIGHT, LEFT, DOWN};
        private direction _direction;



        #endregion

        #region Properties 
        //Pontszam lekerdezese
        public Int32 Score { get { return _player.Score; } }
        //eletek lekerdezese
        public Int32 Lives { get { return _player.Lives; } }
        public Int32 Rounds { get { return _rounds; } }
        public Int32 ActiveIndividual { get { return _network.ActiveIndividual; } }
        //jatek vege lekerdezese
        public Boolean IsGameOver { get { return (_player.Lives == 0); } }
        //hajo y pos lekerdezese vege lekerdezese
        public int XPos { get { return _player.XPos; } }
        // irany beallitasa
        public void GoLeft(bool goLeft) { _player.GoLeft = goLeft; }
        public void GoRight(bool goRight) { _player.GoRight = goRight; }
        //bullet
        public void BulletOn(bool bullet) { _player.Bullet = bullet; }
        public bool NetworkOn { get { return _network._networkOn; } set { _network._networkOn = value; } }


        #endregion

        #region Constructor
        public GameModel(IGameDataAccess dataAccess)
        {
            //_enemys = new EnemyStruct[_enemyRows, _enemyColumns];
            _enemyTable = new EnemyTable();
            _bullets = new Bullet[_maxBullet];
            _player = new Player();
            _network = new NeuralNetwork(_hiddenNeuronSize,_populationSize);
            _network._networkOn = false;
            _dataAccess = dataAccess;
            _viewOn = true;
            ReSetBulletTable();
            _timer = new System.Timers.Timer(20);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Stop();
            _timerOffView = new System.Timers.Timer(1);
            _timerOffView.Elapsed += _timerOffView_Elapsed;
            _timerOffView.AutoReset = true;
            _timerOffView.Enabled = true;
            _timerOffView.Stop();
            _rounds = 0;
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            _startGame = true;
            _timer.Stop();
            _timerOffView.Stop();
            //ReSetEnemyTable();
            _enemyTable.ReSetTable(_windowWidth);
            ReSetBulletTable();
            _player.XPos = 312;
            _player.Score = 0;
            _player.Lives = 1;
            _player.Bullet = false;
            _bulletCount = 0;
            //_enemysCount = _maxenemyCount;
            //_enemyBullet.Alive = false;
            //_enemyBullet.IsNewBullet = false;
            //_enemyBulletTimeCounter = 0;
            _player.GoLeft = false;
            _player.GoRight = false;
            _enemyTable.Speed = _enemyTable.BasicSpeed;
            //_enemySpeed = _enemyBasicSpeed;
            //_direction= direction.RIGHT;
            _rounds++;
            OnGameCreated();
            if (_viewOn)
            {
                _timer.Start();
            }
            else
            {
                _timerOffView.Start();
            }
            if (_network._networkOn)
            {
                _network.ReSetFittnes();
            }
        }
        public void NewRound()
        {
            _timer.Stop();
            //ReSetEnemyTable();
            _enemyTable.ReSetTable(_windowWidth);
            ReSetBulletTable();
            _player.XPos = 312;
            _player.Bullet = false;
            _bulletCount = 0;
            //_enemysCount = _maxenemyCount;
            _direction = direction.RIGHT;
            _player.GoLeft = false;
            _player.GoRight = false;
            OnGameCreated();
            if (_viewOn)
            {
                _timer.Start();
            }
            else
            {
                _timerOffView.Start();
            }
        }

        // Jatek betoltese
        public async Task LoadNetworkAsync(String path)
        {
            
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            Data data = await _dataAccess.LoadAsync(path);
            _rounds = data._round;
            _populationSize = data._populationSize;
            _network._networkOn = true;
            //_hiddenNeuronSize = data._weightsSize;
            _network.LoadNetwork(data);
            GameOver(this, new GameOverEventArgs(false, _network._networkOn));
            //NewGame();
            NetworkLoaded?.Invoke(this, new GameEventArgs(_player.Score, _player.Lives, _player.XPos, _bullets, _enemyTable.Enemies, _enemyTable.Bullet, _network.ActiveIndividual));
        }

        // neuralos halok mentese mentése
        public async Task SaveNetworkAsync(String path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            if (_network.EvolutionType == NeuralNetwork.evolution.SIMPLE)
            {
                await _dataAccess.SaveAsync(path, 0, _rounds, _populationSize, _network.WeightsCount, _network.Weights, _network.IndividualFittnes, _network.LearningTime, _network.IndividualScore, _network.EvolutionParameters);
            }
            else
            {
                await _dataAccess.SaveAsync(path, 1, _rounds, _populationSize, _network.WeightsCount, _network.Weights, _network.IndividualFittnes, _network.LearningTime, _network.IndividualScore, _network.EvolutionParameters);
            }
            
        }

        public void stopTimer()
        {
            if (_viewOn)
            {
                _timer.Stop();
            }
            else
            {
                _timerOffView.Stop();
            }
        }
        public void startTimer()
        {
            if (_viewOn)
            {
                _timer.Start();
            }
            else
            {
                _timerOffView.Start();
            }
        }
        public void ChangeManual()
        {
            _player.GoLeft = false;
            _player.GoRight = false;
            _player.Bullet = false;
            _network.ActiveIndividual = -1;
            NetworkOn = false;
        }
        public void ChangeAI()
        {
            _player.GoLeft = false;
            _player.GoRight = false;
            _player.Bullet = false;
            _network.ActiveIndividual = 0;
            NetworkOn = true;
        }

        public void BestPlay()
        {
            _network.BestPlay();
        }

        public void TurnOffView()
        {
            if (NetworkOn)
            {
                _timer.Stop();
                _viewOn = false;
                _timerOffView.Start();
            }
        }

        public void TurnOnView()
        {
            _timerOffView.Stop();
            _timer.Start();
            _viewOn = true;
        }        
        public void TurnSimpleEvolution()
        {
            if (_network.EvolutionType != NeuralNetwork.evolution.SIMPLE)
            {
                _network.TurnSimpleEvolution();
                _rounds = _network.Round;
                
                OnGameOver(false);
            }
        }
        public void TurnRedQueenEvolution()
        {
            if (_network.EvolutionType != NeuralNetwork.evolution.REDQUEEN)
            {
                _network.TurnRedQueenEvolution();
                _rounds = _network.Round;
                OnGameOver(false);
            }
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
            _player.Move(_windowBorder, _windowWidth);
            _enemyTable.BulletTimeCounter++;
            bool hit = false;
            bool avoid = false;
            (hit, avoid) = _enemyTable.BulletMove(_player.YPos, _player.XPos, _player.Width, _windowHeight);
            if (hit)
            {
                _player.Lives--;
            }
            if (avoid)
            {
                _network._avoidBullets++;
            }
            //EnemyBulletMove();
            if (_enemyTable.BulletTimeCounter >= _enemyTable.BulletTimeDistance)
            {;
                _enemyTable.CreateBullet(_player.XPos, _windowBorder, _player.Width);
                //CreateEnemyBullet();
            }
            _enemyTable.Move(_windowBorder);
            //EnemyMove();
            BulletMove();
            CreateBullet();
            OnGameAdvanced();
            _network._elapsedTime += 0.02D;
            if (GameOverIs())
            {
                OnGameOver(_win);
            }

        }        
        private void _timerOffView_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NetworkAction();
            _player.Move(_windowBorder, _windowWidth);
            _enemyTable.BulletTimeCounter++;
            bool hit = false;
            bool avoid = false;
            (hit, avoid) = _enemyTable.BulletMove(_player.YPos, _player.XPos, _player.Width, _windowHeight);
            if (hit)
            {
                _player.Lives--;
            }
            if (avoid)
            {
                _network._avoidBullets++;
            }
            //EnemyBulletMove();
            if (_enemyTable.BulletTimeCounter >= _enemyTable.BulletTimeDistance)
            {
                _enemyTable.CreateBullet(_player.XPos, _windowBorder, _player.Width);
                //CreateEnemyBullet();
            }
            _enemyTable.Move(_windowBorder);
            //EnemyMove();
            BulletMove();
            CreateBullet();
            //OnGameAdvanced();
            _network._elapsedTime += 0.02D;
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
        /*
        private void ReSetEnemyTable()
        {
            int enemyColumn = 0;
            //int s = _windowWidth - ((_enemyColumns * _enemySize) + ((_enemyColumns - 1) * _enemyDistance));
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
        */
        private void CreateBullet()
        {
            if (_player.Bullet == true)
            {
                _player.Bullet = false;
                _bullets[_bulletCount].X = _player.XPos + _player.Width/2-2;
                _bullets[_bulletCount].Y = _player.YPos - _bulletHight-6;
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
                _network._usedBullets++;
            }
        }
        /*
        private void CreateEnemyBullet()
        {
            _enemyBullet.IsNewBullet = true;
            _enemyBullet.Alive = true;
            _enemyBullet.X = _player.XPos + (_player.Width / 2)-2;
            _enemyBullet.Y = _windowBorder + _bulletHight;
            //_enemyBullet.Y = _enemyButtomYPos-_enemySize;
        }
        */
        /*
        private void EnemyBulletMove()
        {
            if (_enemyBullet.Alive)
            {
                _enemyBullet.IsNewBullet = false;
                _enemyBullet.Y += _bulletspeed;
                if ((_enemyBullet.Y + _bulletHight >= _player.YPos && _enemyBullet.X >= _player.XPos && _enemyBullet.X <= _player.XPos + _player.Width) && _enemyBullet.Alive )
                {
                    _player.Lives--;
                    _enemyBullet.Alive = false;
                }else if(_enemyBullet.Y + _bulletHight +5 >= _windowHeight - 95){
                    _enemyBullet.Alive = false;
                    _network._avoidBullets++;
                }
            }
        }
        */
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
                    for (int j = 0; j < _enemyTable.Rows; j++)
                    {
                        for (int z = 0; z < _enemyTable.Columns; z++)
                        {
                            if (_enemyTable.Enemies[j, z].Alive == true && _enemyTable.Enemies[j, z].X() <= _bullets[i].X && _enemyTable.Enemies[j, z].X() + 45 >= _bullets[i].X && _enemyTable.Enemies[j, z].Y() >= _bullets[i].Y - 45 && _enemyTable.Enemies[j, z].Y() <= _bullets[i].Y)
                            {
                                _bullets[i].Alive = false;
                                _enemyTable.Enemies[j, z].Alive = false;
                                _enemyTable.Count--;
                                if (_enemyTable.Count == 1)
                                {
                                    _enemyTable.Speed++; 
                                    //_enemySpeed++;
                                }
                                switch (_enemyTable.Enemies[j, z].Type)
                                {
                                    case EnemyStruct.enyemyType.RED:
                                        _player.Score += 15;
                                        break;
                                    case EnemyStruct.enyemyType.ORANGE:
                                        _player.Score += 10;
                                        break;
                                    case EnemyStruct.enyemyType.BLUE:
                                        _player.Score += 5;
                                        break;
                                }
                                if ((j,z)==(_enemyTable.MostButtomEnemySerial))
                                {
                                    _enemyTable.NewMostDown();
                                    //NewMostDown();
                                }
                                if ((j, z) == (_enemyTable.MostRightEnemySerial))
                                {
                                    _enemyTable.NewMostRight();
                                    //NewMostRight();
                                }
                                if ((j, z) == (_enemyTable.MostLeftEnemySerial))
                                {
                                    _enemyTable.NewMostLeft();
                                    //NewMostLeft();
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

        //enemy move methods
        /*
        private void EnemyMove()
        {
            if(_direction == direction.RIGHT && _enemyTable.MostRightCoord() < 625 )
            {
                _enemyTable.MoveRight();
            }
            else if(_direction == direction.RIGHT && _enemyTable.MostRightCoord() >= 625)
            {
                _direction = direction.LEFT;
            }
            if(_direction == direction.LEFT && _enemyTable.MostLeftCoord()> _windowBorder)
            {
                MoveLeft();
            }
            else if (_direction == direction.LEFT && _enemyTable.MostLeftCoord() <= _windowBorder)
            {
                _direction = direction.DOWN;
            }
            else if(_direction == direction.DOWN)
            {
                MoveDown();
                _direction = direction.RIGHT;
            }
            _enemyButtomYPos = _enemyTable.MostLeftDown()+_enemySize;
        }
        */
        /*
        private double MostRightCoord()
        {
            int x; int y;
            (x, y) = _enemyTable.MostRightEnemySerial;
            double max = _enemyTable.Enemies[x, y].X();
            return max;
        }        
        private double MostLeftCoord()
        {
            int x; int y;
            (x, y) = _mostLeftEnemySerial;
            double max = _enemyTable.Enemies[x, y].X();
            return (max);
        }  */      
        /*private int MostLeftDown()
        {
            int x; int y;
            (x,y) = _mostButtomEnemySerial;
            int max = _enemyTable.Enemies[x, y].Y()+_enemySize;
            return (max);
        }
        private void MoveLeft()
        {
            double s;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    s = _enemyTable.Enemies[i, j].X();
                    _enemyTable.Enemies[i, j].X(s - _enemyTable.Speed);
                }
            }
        }
        private void MoveRight()
        {
            double s;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    s = _enemyTable.Enemies[i, j].X();
                    _enemyTable.Enemies[i, j].X(s + _enemyTable.Speed);
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
                    s = _enemyTable.Enemies[i, j].Y();
                    _enemyTable.Enemies[i, j].Y(s + _enemySize);
                }
            }
        }
        /*
        private void NewMostRight()
        {
            double max = -1;
            int maxI = 0;
            int maxJ = 0;
            for(int i = 0; i < _enemyRows; i++)
            {
                for(int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemyTable.Enemies[i, j].X() > max && _enemyTable.Enemies[i,j].Alive) {
                        max = _enemyTable.Enemies[i, j].X();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostRightEnemySerial = (maxI,maxJ);
        }*//*
        private void NewMostLeft()
        {
            double min = 1000;
            int minI = 0;
            int minJ = 0;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemyTable.Enemies[i, j].X() < min && _enemyTable.Enemies[i, j].Alive)
                    {
                        min = _enemyTable.Enemies[i, j].X();
                        minI = i;
                        minJ = j;
                    }
                }
            }
            _mostLeftEnemySerial = (minI, minJ);
        }
        private void NewMostDown()
        {
            double max = 0;
            int maxI = 0;
            int maxJ = 0;
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (_enemyTable.Enemies[i, j].Y() > max && _enemyTable.Enemies[i, j].Alive)
                    {
                        max = _enemyTable.Enemies[i, j].Y();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostButtomEnemySerial = (maxI, maxJ);
        }
        */
        private void NetworkAction()
        {
            if (_network._networkOn)
            {
                SetNetwork();
                NeuralNetwork.action nextAction = _network.NextAction();
                switch (nextAction)
                {
                    case NeuralNetwork.action.GORIGHT:
                        _player.GoLeft = false;
                        _player.GoRight = true;
                        _network._goLeft = false;
                        _network._goRight = true;
                        break;
                    case NeuralNetwork.action.GOLEFT:
                        _player.GoLeft = true;
                        _player.GoRight = false;
                        _network._goLeft = true;
                        _network._goRight = false;
                        break;
                    case NeuralNetwork.action.SHOT:
                        _player.GoLeft = false;
                        _player.GoRight = false;
                        _network._goLeft = false;
                        _network._goRight = false;
                        _player.Bullet = true;
                        break;
                }
            }
        }
        private void SetNetwork()
        {
            if (_enemyTable.Bullet.Alive)
            {
                _network._bulletDistance = (_player.YPos - _enemyTable.Bullet.Y - _bulletHight)/700D; //enemy bullet tavolsaga
            }
            else
            {
                _network._bulletDistance = 0;
            }
            if(_enemyTable.Bullet.Alive)
            {
                if(_enemyTable.Bullet.X > _player.XPos)
                {
                    _network._bulletXRightDistance = (_enemyTable.Bullet.X - _player.XPos - _player.Width / 2)/700D;
                }
                else
                {
                    _network._bulletXRightDistance = 0;
                }
                if (_enemyTable.Bullet.X < _player.XPos)
                {
                    _network._bulletXLeftDistance =(_player.XPos - _enemyTable.Bullet.X + _player.Width / 2)/700D;
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
            _network._enemyCount = _enemyTable.Count / 50D;
            int x, y; 
            (x,y)= _enemyTable.MostButtomEnemySerial;
            _network._closestEnemyYDistance = (_player.YPos - _enemyTable.Enemies[x,y].Y() - _enemyTable.Size)/580D;
            _network._closestEnemyXDistance = Math.Abs( (_player.XPos + _player.Width / 2) - (_enemyTable.Enemies[x, y].X() + _enemyTable.Size / 2))/628D;
            if(_network._closestEnemyXDistance == (_player.XPos + _player.Width / 2) - (_enemyTable.Enemies[x, y].X() + _enemyTable.Size / 2))
            {
                _network._closestEnemyDirection = 0;
            }
            else
            {
                _network._closestEnemyDirection = 1;
            }
            _network._lives = _player.Lives / 2;
            _network._leftDistanc = (_player.XPos - 7)/628D;
            _network._rightDistanc = (627- _player.XPos) /628D;
            _network._rightEnemyCount = RightEnemyCount()/50D;
            _network._leftEnemyCount = LeftEnemyCount()/50D;

        }
        private int RightEnemyCount()
        {
            int count = 0;
            for (int i=0; i < _enemyTable.Rows; i++) 
            {
                for(int j=0; j < _enemyTable.Columns; j++)
                {
                    if (_enemyTable.Enemies[i,j].Alive && _enemyTable.Enemies[i,j].X() > _player.XPos + _player.Width / 2)
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
            for (int i = 0; i < _enemyTable.Rows; i++)
            {
                for (int j = 0; j < _enemyTable.Columns; j++)
                {
                    if (_enemyTable.Enemies[i, j].Alive && _enemyTable.Enemies[i, j].X() < _player.XPos+ _player.Width / 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        private bool GameOverIs()
        {
            if (_player.Lives == 0) {
                _win = false;
                return true;
            }
            if(_enemyTable.ButtomYPos > _player.YPos)
            {
                 _win = false;
                return true;
            }
            if (_enemyTable.Count == 0)
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
                GameAdvanced(this, new GameEventArgs(_player.Score, _player.Lives, _player.XPos, _bullets, _enemyTable.Enemies, _enemyTable.Bullet, _network.ActiveIndividual));
        }
        //enemy tabla letrehozasanak esemenye
        public void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new EnemyEventArgs(_enemyTable.Enemies, _enemyTable.Columns, _enemyTable.Rows, _enemyTable.Size, _enemyTable.Count, _maxBullet, _bullets));
            
        }

        //jatek vege
        public void OnGameOver(bool win)
        {
            _timer.Stop();
            if (_network._networkOn) {
                _network.GameOver(_player.Score, win);
            }
                if (GameOver != null)
                GameOver(this, new GameOverEventArgs(win, _network._networkOn));
        }

        #endregion


    }
}

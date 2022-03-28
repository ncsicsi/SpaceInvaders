//#define RIGHT 0
//#define LEFT 1
//#define DOWN 2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SpaceInvaders.Model
{

    internal class GameModel
    {

        /*
        #region public Structs 
        public struct Enemy
        {
            bool _alive;
            int _x;
            int _y;
            int _type;   // 1-3 tipus
            public void Alive(bool alive) { _alive = alive; }
            public bool Alive() { return _alive; }
            public void Type(int type) { _type = type; }
            public int Type() { return _type; }

            public void X(int x) { _x=x; }
            public void Y(int y) { _y = y; }
            public int X() {   return _x;  }
            public int Y() {  return _y;  }
        }


        public struct Bullet
        {
            private int _bulletX;
            private int _bulletY;
            //private bool _bullet;
            private bool _bulletAlive;
            public int X { get { return _bulletX; } set { _bulletX = value; } }
            public int Y { get { return _bulletY; } set { _bulletY = value; } }
            public bool Alive { get { return _bulletAlive; } set { _bulletAlive = value; } }
        }
        #endregion
        */

        #region Fields
        private int _score;
        private int _lives;
        private int _enemysCount;
        private EnemyStruct[,] _enemys = new EnemyStruct[5, 10];
        private Bullet[]  _bullets= new Bullet[15];
        private int _shipXPos;
        private int _shipYPos;
        private bool _goLeft;
        private bool _goRight;
        private bool _bullet;
        private static System.Timers.Timer _timer;
        private static int _windowWidth = 500;
        private static int _windowHeight = 700;
        private static int _shipWidth = 104;
        private static int _shipHeight = 63;
        private static int _enemySize = 45;
        private int _bulletCount;
        private int _enemySpeed;
        private int _enemyBasicSpeed;
        private int _enemyBulletTimeDistance;   //milyen idokozonkent lonek az enemyk
        private int _enemyBulletTimeCounter;
        private Bullet _enemyBullet;
        private int _bulletHight;
        private bool _win;
        private int _bulletspeed;
        private int _enemyButtomYPos;
        private (int, int) _mostRightEnemySerial;
        private (int, int) _mostLeftEnemySerial;
        private (int, int) _mostButtomEnemySerial;
        public enum direction {RIGHT, LEFT, DOWN};
        private direction _direction;
        

        #endregion

        #region Properties 
        //Pontszam lekerdezese
        public Int32 Score { get { return _score; } }
        //eletek lekerdezese
        public Int32 Lives { get { return _lives; } }
        //jatek vege lekerdezese
        public Boolean IsGameOver { get { return (_lives == 0); } }
        //hajo y pos lekerdezese vege lekerdezese
        public int XPos { get { return _shipXPos; } }
        // irany beallitasa
        public void GoLeft(bool goLeft) { _goLeft = goLeft; }
        public void GoRight(bool goRight) { _goRight = goRight; }
        //bullet
        public void BulletOn(bool bullet) { _bullet = bullet; }


        #endregion

        #region Constructor
        public GameModel()
        {

            ReSetBulletTable();


        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            ReSetEnemyTable();
            _score = 0;
            _lives = 2;
            _shipXPos = 298;
            _shipYPos = 570;
            _bullet = false;
            _bulletCount = 0;
            _bulletspeed = 10;
            _enemyBulletTimeDistance = 150;
            _enemyBulletTimeCounter = 0;
            _enemyBullet.Alive = false;
            _enemyBullet.IsNewBullet = false;
            _bulletHight = 20;
            _enemysCount = 50;
            _enemyBasicSpeed = 1;
            _enemySpeed = _enemyBasicSpeed;
            _direction= direction.RIGHT;
            _timer = new System.Timers.Timer(5);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
            
        }
        public void NewRound()
        {
            ReSetEnemyTable();
            _shipXPos = 298;
            _bullet = false;
            _bulletCount = 0;
            _enemysCount = 50;
            _enemyBasicSpeed++;
            _enemySpeed = _enemyBasicSpeed;
            _direction = direction.RIGHT;
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }
        public void stopTimer()
        {
            _timer.Stop();

        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_goLeft && _shipXPos > 10)
            {
                _shipXPos -= 5;
            }
            else if (_goRight && _shipXPos < 576)
            {
                _shipXPos += 5;
            }
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

        #endregion

        #region Events

        /// Játék létrehozásának eseménye.
        public event EventHandler<EnemyEventArgs> GameCreated;
        // Játék végének eseménye.
        public event EventHandler<GameOverEventArgs> GameOver;
        //Jatek előrehaladáskor frissitesi esemeny
        public event EventHandler<GameEventArgs> GameAdvanced;
        //Enemy tabla letrehozasanak esemenye, hogy lekuldjuk a viewba
        

        #endregion

        #region Private Methods

        private void ReSetBulletTable()
        {
            for(int i = 0; i < 15; i++)
            {
                _bullets[_bulletCount].Alive = false;
            }
        }


        private void ReSetEnemyTable()
        {
            int enemyColumn = 0;
            int left = 560;
            for (int i = 0; i < _enemys.Length; i++)
            {
                switch (i)
                {
                    case 10:
                        enemyColumn=0;
                        left = 560;
                        break;
                    case 20:
                        enemyColumn=0;
                        left = 560;
                        break;
                    case 30:
                        enemyColumn=0;
                        left = 560;
                        break;
                    case 40:
                        enemyColumn=0;
                        left = 560;
                        break;
                }
                int type = 1;
                if (i < 10) type = 1;
                else if(i < 30) type = 2;
                else if(i < 50) type = 3;

                    _enemys[(i/10), enemyColumn].Alive(true);
                    _enemys[i / 10, enemyColumn].Type(type);
                    _enemys[i / 10, enemyColumn].Y(10+(i/10)*(_enemySize+10));
                    _enemys[i / 10, enemyColumn].X(left);

                enemyColumn++;
                left -= (_enemySize+10);
            }
            _mostButtomEnemySerial = (4, 0);
            _mostRightEnemySerial = (0,0);
            _mostLeftEnemySerial = (0, 9);
            _enemyButtomYPos = _enemys[4, 0].Y() + _enemySize;
            OnGameCreated();

        }

        private void CreateBullet()
        {
            if (_bullet == true)
            {
                _bullet = false;
                _bullets[_bulletCount].X = _shipXPos + 50;
                _bullets[_bulletCount].Y = 550;
                _bullets[_bulletCount].Alive = true;
                _bullets[_bulletCount].IsNewBullet = true;
                if (_bulletCount < 14)
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
            _enemyBullet.X = _shipXPos+(_shipWidth/2);
            _enemyBullet.Y = _enemyButtomYPos-_enemySize;
        }

        private void EnemyBulletMove()
        {
            if (_enemyBullet.Alive)
            {
                _enemyBullet.IsNewBullet = false;
                _enemyBullet.Y += _bulletspeed;
                if (_enemyBullet.Y + _bulletHight >= _shipYPos && _enemyBullet.X>= _shipXPos && _enemyBullet.X <= _shipXPos + _shipWidth)
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
            for (int i = 0; i < 15; i++)
            {
                if (_bullets[i].Alive)
                {
                    _bullets[i].IsNewBullet = false;
                    _bullets[i].Y -= _bulletspeed;
                    if (_bullets[i].Y == 0)
                    {
                        _bullets[i].Alive = false;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            if (_enemys[j, z].Alive() == true && _enemys[j, z].X() <= _bullets[i].X && _enemys[j, z].X() + 45 >= _bullets[i].X && _enemys[j, z].Y() >= _bullets[i].Y - 45 && _enemys[j, z].Y() <= _bullets[i].Y)
                            {
                                _bullets[i].Alive = false;
                                _enemys[j, z].Alive(false);
                                _enemysCount--;
                                if (_enemysCount == 1)
                                {
                                    _enemySpeed++;
                                }
                                switch (_enemys[j, z].Type())
                                {
                                    case 1:
                                        _score += 15;
                                        break;
                                    case 2:
                                        _score += 10;
                                        break;
                                    case 3:
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
                }
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
            if(_direction == direction.LEFT && MostLeftCoord()>15 )
            {
                MoveLeft();
            }
            else if (_direction == direction.LEFT && MostLeftCoord() <= 15)
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
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s - _enemySpeed);
                }
            }
        }
        private void MoveRight()
        {
            int s;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s + _enemySpeed);
                }
            }
        }
        private void MoveDown()
        {
            int s;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    s = _enemys[i, j].Y();
                    _enemys[i, j].Y(s + 45);
                }
            }
        }

        private void NewMostRight()
        {
            int max = -1;
            int maxI = 0;
            int maxJ = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if (_enemys[i, j].X() > max && _enemys[i,j].Alive()) {
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
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_enemys[i, j].X() < min && _enemys[i, j].Alive())
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
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_enemys[i, j].Y() > max && _enemys[i, j].Alive())
                    {
                        max = _enemys[i, j].X();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostButtomEnemySerial = (maxI, maxJ);
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
                GameCreated(this, new EnemyEventArgs(_enemys));
            
        }

        //jatek vege
        private void OnGameOver(bool win)
        {
            _timer.Stop();
            if (GameOver != null)
                GameOver(this, new GameOverEventArgs(win));
        }

        #endregion

    }
}

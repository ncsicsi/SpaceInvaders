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
        private int _invadiersSpeed;
        private int _enemysCount;
        //private Enemy[,] _enemys = new Enemy[5, 10];
        private EnemyStruct[,] _enemys = new EnemyStruct[5, 10];
        private Bullet[]  _bullets= new Bullet[15];
        private int _shipXPos;
        private bool _goLeft;
        private bool _goRight;
        private bool _bullet;
        private static System.Timers.Timer _timer;
        private static int _windowWidth = 500;
        private static int _windowHeight = 700;
        private static int _shipWidth = 104;
        private static int _shipHeight = 63;
        private int _bulletCount;

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
        //public EnemyStruct[,] Enemys { get { return _enemys; } set { _enemys = value; } }
        /*public int BulletX { get { return _bulletX; } }
        public int BulletY { get { return _bulletY; } }
        */

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
            _invadiersSpeed = 10;
            _shipXPos = 298;
            _bullet = false;
            _bulletCount = 0;
            _enemysCount = 50;
            _timer = new System.Timers.Timer(10);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
            
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
            if(_bullet == true)
            {
                _bullet = false;
                _bullets[_bulletCount].X = _shipXPos;
                _bullets[_bulletCount].Y = 550;
                _bullets[_bulletCount].Alive = true;
                if (_bulletCount < 15)
                {
                    _bulletCount++;
                }
                else
                {
                    _bulletCount = 0;
                }
            }
            for(int i = 0; i < 15; i++)
            {
                if (_bullets[i].Alive)
                {
                    _bullets[i].Y -= 20;
                    if (_bullets[i].Y == 0)
                    {
                        _bullets[i].Alive = false;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            if (_enemys[j, z].Alive() == true && _enemys[j, z].X() <= _bullets[i].X && _enemys[j, z].X() + 45 >= _bullets[i].X && _enemys[j, z].Y() == _bullets[i].Y)
                            {
                                _bullets[i].Alive = false;
                                _enemys[j, z].Alive(false);
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
                            }
                        }
                    }
                }
            }
            
            OnGameAdvanced();
        }

        #endregion

        #region Events

        /// Játék létrehozásának eseménye.
        //public event EventHandler<GameEventArgs> GameCreated;
        // Játék végének eseménye.
        public event EventHandler<GameEventArgs> GameOver;
        //Jatek előrehaladáskor frissitesi esemeny
        public event EventHandler<GameEventArgs> GameAdvanced;
        //Enemy tabla letrehozasanak esemenye, hogy lekuldjuk a viewba
        public event EventHandler<EnemyEventArgs> GameCreated;

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
                if (i < 11)
                {
                    _enemys[0, enemyColumn].Alive(true);
                    _enemys[0, enemyColumn].Type(1);
                    _enemys[0, enemyColumn].Y(10);
                    _enemys[0, enemyColumn].X(left);
                }
                else if (i < 21)
                {
                    _enemys[0, enemyColumn].Alive(true);
                    _enemys[0, enemyColumn].Type(2);
                    _enemys[0, enemyColumn].Y(55+10);
                    _enemys[0, enemyColumn].X(left);
                }
                else if (i < 31)
                {
                    _enemys[0, enemyColumn].Alive(true);
                    _enemys[0, enemyColumn].Type(2);
                    _enemys[0, enemyColumn].Y(2*55 + 10);
                    _enemys[0, enemyColumn].X(left);
                }
                else if (i < 41)
                {
                    _enemys[0, enemyColumn].Alive(true);
                    _enemys[0, enemyColumn].Type(3);
                    _enemys[0, enemyColumn].Y(3 * 55 + 10);
                    _enemys[0, enemyColumn].X(left);
                }
                else if (i < 51)
                {
                    _enemys[0, enemyColumn].Alive(true);
                    _enemys[0, enemyColumn].Type(3);
                    _enemys[0, enemyColumn].Y(4 * 55 + 10);
                    _enemys[0, enemyColumn].X(left);
                }
                enemyColumn++;
                left -= 55;
                /*switch (i)
                {
                    case 0:
                        for (int j = 0; j < 10; j++) // 1. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(1);
                            _enemys[i, j].Y(i*55 + 10);
                            _enemys[i, j].X(560-55*j);
                        }
                        break;
                    case 1:
                        for (int j = 0; j < 10; j++) // 2. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(2);
                            _enemys[i, j].Y(i * 55 + 10);
                            _enemys[i, j].X(560 - 55 * j);
                        }
                        break;
                    case 2:
                        for (int j = 0; j < 10; j++) // 3. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(2);
                            _enemys[i, j].Y(i * 55 + 10);
                            _enemys[i, j].X(560 - 55 * j);
                        }
                        break;
                    case 3:
                        for (int j = 0; j < 10; j++) // 4. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(3);
                            _enemys[i, j].Y(i * 55 + 10);
                            _enemys[i, j].X(560 - 55 * j);
                        }
                        break;
                    case 4:
                        for (int j = 0; j < 10; j++) // 5. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(3);
                            _enemys[i, j].Y(i * 55 + 10);
                            _enemys[i, j].X(560 - 55 * j);
                        }
                        break;
                }*/

            }
            OnGameCreated();

        }
        //elorehaladasa a jateknak es frissites
        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
                GameAdvanced(this, new GameEventArgs(_score, _lives, _shipXPos));
        }
        //enemy tabla letrehozasanak esemenye
        private void OnGameCreated()
        {
            if (GameCreated != null)
                GameCreated(this, new EnemyEventArgs(_enemys));
            
        }

        //jatek vege
        private void OnGameOver()
        {
            if (GameOver != null)
                GameOver(this, new GameEventArgs(_score, _lives, _shipXPos));
        }

        #endregion

    }
}

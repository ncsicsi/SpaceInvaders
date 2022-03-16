using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    internal class GameModel
    {
        #region Enemy 
        public struct Enemy
        {
            bool _alive;
            int _x;
            int _y;
            int _type;   // 1-3 tipus
            public void Alive(bool alive) { _alive = alive; }
            public void Type(int type) { _type = type; }
        }
        #endregion

        #region Fields
        private int _score;
        private int _lives;
        private int _invadiersSpeed;
        private int _invdiersCount;
        private Enemy[,] _enemys = new Enemy[5,10];
        private int _shipXPos;
        private bool _goLeft;
        private bool _goRight;
        private static System.Timers.Timer _timer;
        private static int _windowWidth = 500;
        private static int _windowHeight = 700;
        private static int _shipWidth = 104;
        private static int _shipHeight = 63;

        #endregion

        #region Properties 
        //Pontszam lekerdezese
        public Int32 Score { get { return _score; } }
        //eletek lekerdezese
        public Int32 Lives { get { return _lives; } }
        //jatek vege lekerdezese
        public Boolean IsGameOver { get { return (_lives == 0); } }
        //hajo y pos lekerdezese vege lekerdezese
        public int XPos { get { return _shipXPos ;} }
        // irany beallitasa
        public void GoLeft(bool goLeft) { _goLeft = goLeft;  }
        public void GoRight(bool goRight) { _goRight = goRight;  }

        #endregion

        #region Constructor
        public GameModel()
        {
            ReSetEnemyTable();
            
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            _score = 0; 
            _lives = 2; 
            _invadiersSpeed = 10;
            _shipXPos = 298;
            _timer = new System.Timers.Timer(20);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_goLeft && _shipXPos > 15)
            {
                _shipXPos -= 10;
            }
            else if (_goRight && _shipXPos < 581)
            {
                _shipXPos += 10;
            }
        }

        #endregion

        #region Events

        /// Játék létrehozásának eseménye.
        public event EventHandler<GameEventArgs> GameCreated;
        // Játék végének eseménye.
        public event EventHandler<GameEventArgs> GameOver;
        //Jatek előrehaladáskor frissitesi esemeny
        public event EventHandler<GameEventArgs> GameAdvanced;

        #endregion

        #region Private Methods
        //jatek vege
        private void OnGameOver()
        {
            if (GameOver != null)
                GameOver(this, new GameEventArgs(_score, _lives, _shipXPos));
        }

        private void ReSetEnemyTable()
        {
            for(int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < 10; j++) // 1. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(1);

                        }
                        break;
                        case 1:
                        for (int j = 0; j < 10; j++) // 2. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(2);

                        }
                        break;
                    case 2:
                        for (int j = 0; j < 10; j++) // 3. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(2);

                        }
                        break;
                    case 3:
                        for (int j = 0; j < 10; j++) // 4. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(3);

                        }
                        break;
                    case 4:
                        for (int j = 0; j < 10; j++) // 5. Sor
                        {
                            _enemys[i, j].Alive(true);
                            _enemys[i, j].Type(3);

                        }
                        break;
                }

            }

        }

        #endregion

    }
}

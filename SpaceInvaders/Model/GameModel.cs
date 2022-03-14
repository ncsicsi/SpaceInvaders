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
        }
        #endregion


        #region Fields
        private int _score;
        private int _lives;
        private int _invadiersSpeed;
        private int _invdiersCount;
        private Enemy[][] _enemys;
        private int _shipYPos;

        #endregion

        #region Properties 
        //Pontszam lekerdezese
        public Int32 Score { get { return _score; } }
        //eletek lekerdezese
        public Int32 Lives { get { return _lives; } }
        //jatek vege lekerdezese
        public Boolean IsGameOver { get { return (_lives == 0); } }
        //hajo y pos lekerdezese vege lekerdezese
        public int YPos { get { return _shipYPos ;} }
        #endregion

        #region Constructor
        public GameModel()
        {
            //_enemys = new Enemy[5,11];
            ReSetEnemyTable();
            
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            _score = 0; 
            _lives = 2; 
            _invadiersSpeed = 10;
            _shipYPos = 375;
            
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
                GameOver(this, new GameEventArgs(_score, _lives, _shipYPos));
        }

        private void ReSetEnemyTable()
        {
            for(int i = 0; i < 11; i++)
            {
                if (i == 0)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        //_enemys[i,j] = _enemys[i,j];
                         
                    }
                }
            }

        }

        #endregion

    }
}

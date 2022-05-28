using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class GameEventArgs : EventArgs
    {
        //játék vége eseményhez
        private int _score;
        private int _lives;
        private int _xPos;
        private Bullet[] _bullets;
        private EnemyStruct[,] _enemies;
        private Bullet _enemyBullet;
        private int _activeIndividual;

        //pontszám lekérdezése
        public int Score { get { return _score; } }
        public int Lives { get { return _lives; } }
        public int YPos { get { return _xPos; } }
        public Bullet[] Bullets { get { return _bullets; } }
        public EnemyStruct[,] Enemies { get { return _enemies; } }
        public int ActiveIndividual { get { return _activeIndividual; } }
        public Bullet EnemyBullet { get { return _enemyBullet; } }

        #region Constructor
        public GameEventArgs(int score, int lives, int xPos, Bullet[] bullets, EnemyStruct[,] enemies, Bullet enemyBullet, int activeIndividual)
        {
            _score = score;
            _lives = lives;
            _xPos = xPos;
            _bullets = bullets;
            _enemies = enemies;
            _enemyBullet = enemyBullet;
            _activeIndividual = activeIndividual;
        }
        #endregion
    }
}

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
        private Bullet[] _bullets = new Bullet[15];
        private EnemyStruct[,] _enemies = new EnemyStruct[5, 10];
        private Bullet _enemyBullet;

        //pontszám lekérdezése
        public int Score { get { return _score; } }
        public int Lives { get { return _lives; } }
        public int YPos { get { return _xPos; } }
        public Bullet[] Bullets { get { return _bullets; } }
        public EnemyStruct[,] Enemies { get { return _enemies; } }
        public Bullet EnemyBullet { get { return _enemyBullet; } }

        #region Constructor
        public GameEventArgs(int score, int lives, int xPos, Bullet[] bullets, EnemyStruct[,] enemies, Bullet enemyBullet)
        {
            _score = score;
            _lives = lives;
            _xPos = xPos;
            _bullets = bullets;
            _enemies = enemies;
            _enemyBullet = enemyBullet;
        }
        #endregion
    }
}

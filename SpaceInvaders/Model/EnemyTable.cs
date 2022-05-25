using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Model
{
    public class EnemyTable
    {
        #region Fields
        private EnemyStruct[,] _enemys ;
        private static int _maxCount = 50;
        private static int _size = 45;
        private static int _columns = 10;
        private static int _rows = 5;
        private static int _distance = 10;
        private int _count = 50;
        private double _basicSpeed = 1;
        private int _bulletTimeDistance = 300;   //milyen idokozonkent lonek az enemyk 300
        private int _bulletTimeCounter;
        private Bullet _bullet;
        private double _speed;
        private int _buttomYPos;   //legalso enemy helyzete x szerint
        private (int, int) _mostRightEnemySerial;
        private (int, int) _mostLeftEnemySerial;
        private (int, int) _mostButtomEnemySerial;
        public enum direction { RIGHT, LEFT, DOWN };
        private direction _direction;
        #endregion

        #region Property

        public EnemyStruct [,] Enemies { get { return _enemys; } }
        public double Speed { get { return _speed; } set { _speed = value; } } 
        public Bullet Bullet { get { return _bullet; } set { _bullet = value; } } 
        public int Size { get { return _size; } set { _size = value; } } 
        public int Columns { get { return _columns; } set { _columns = value; } } 
        public int Rows { get { return _rows; } set { _rows = value; } } 
        public int Count { get { return _count; } set { _count = value; } } 
        public int MaxCount { get { return _maxCount; } set { _maxCount = value; } } 
        public double BasicSpeed { get { return _basicSpeed; } set { _basicSpeed = value; } } 
        public int BulletTimeDistance { get { return _bulletTimeDistance; } set { _bulletTimeDistance = value; } } 
        public int BulletTimeCounter { get { return _bulletTimeCounter; } set { _bulletTimeCounter = value; } } 
        public int ButtomYPos { get { return _buttomYPos; } set { _buttomYPos = value; } } 
        public (int,int) MostRightEnemySerial { get { return _mostRightEnemySerial; } set { _mostRightEnemySerial = value; } } 
        public (int,int) MostLeftEnemySerial { get { return _mostLeftEnemySerial; } set { _mostLeftEnemySerial = value; } } 
        public (int,int) MostButtomEnemySerial { get { return _mostButtomEnemySerial; } set { _mostButtomEnemySerial = value; } } 
        public EnemyTable.direction Direction { get { return _direction; } set { _direction = value; } } 

        #endregion

        #region Constructor
        public EnemyTable()
        {
            _enemys = new EnemyStruct[_rows, _columns];
            _bullet = new Bullet();
            _bullet.Hight = 20;
            _bullet.Speed = 5;
        }
        #endregion

        #region Public Methods

        public void ReSetTable(int windowWidth)
        {
            
            _count = _maxCount;
            _bullet.Alive = false;
            _bullet.IsNewBullet = false;
            _direction = direction.RIGHT;
            int enemyColumn = 0;
            _bulletTimeCounter = 0;
            //int s = _windowWidth - ((_enemyColumns * _enemySize) + ((_enemyColumns - 1) * _enemyDistance));
            int border = windowWidth - (windowWidth - ((_columns * _size) + ((_columns - 1) * _distance))) / 2 - _size;
            int left = border;
            for (int i = 0; i < _enemys.Length; i++)
            {
                switch (i)
                {
                    case 10:
                        enemyColumn = 0;
                        left = border;
                        break;
                    case 20:
                        enemyColumn = 0;
                        left = border;
                        break;
                    case 30:
                        enemyColumn = 0;
                        left = border;
                        break;
                    case 40:
                        enemyColumn = 0;
                        left = border;
                        break;
                }
                EnemyStruct.enyemyType type = EnemyStruct.enyemyType.RED;
                if (i < 10) type = EnemyStruct.enyemyType.RED;
                else if (i < 30) type = EnemyStruct.enyemyType.ORANGE;
                else if (i < 50) type = EnemyStruct.enyemyType.BLUE;

                _enemys[(i / _columns), enemyColumn].Alive = true;
                _enemys[i / _columns, enemyColumn].Type = type;
                _enemys[i / _columns, enemyColumn].Y(_distance + (i / _columns) * (_size + _distance));
                _enemys[i / _columns, enemyColumn].X(left);

                enemyColumn++;
                left -= (_size + _distance);
            }
            _mostButtomEnemySerial = (_rows - 1, 0);
            _mostRightEnemySerial = (0, 0);
            _mostLeftEnemySerial = (0, _columns - 1);
            _buttomYPos = _enemys[_rows - 1, 0].Y() + _size;


        }

        public void CreateBullet(int playerXPos, int windowBorder, int playerWidth)
        {
            _bulletTimeCounter = 0;
            _bullet.IsNewBullet = true;
            _bullet.Alive = true;
            _bullet.X = playerXPos + (playerWidth / 2) - 2;
            _bullet.Y = windowBorder + _bullet.Hight;
        }

        public (bool,bool) BulletMove(int playerYPos, int playerXPos, int playerWidth, int windowHeight)
        {
            bool _hit = false;
            bool avoid = false;
            if (_bullet.Alive)
            {
                _bullet.IsNewBullet = false;
                _bullet.Y += _bullet.Speed;
                if ((_bullet.Y + _bullet.Hight >= playerYPos && _bullet.X >= playerXPos && _bullet.X <= playerXPos + playerWidth && _bullet.Alive))
                {
                    _hit = true;
                    _bullet.Alive = false;
                }
                else if (_bullet.Y + _bullet.Hight + 5 >= windowHeight - 95)
                {
                    _bullet.Alive = false;
                    avoid = true;
                }
            }
            return (_hit, avoid);
        }

        public void Move(int windowBorder)
        {
            if (_direction == direction.RIGHT && MostRightCoord() < 625)
            {
                MoveRight();
            }
            else if (_direction == direction.RIGHT && MostRightCoord() >= 625)
            {
                _direction = direction.LEFT;
            }
            if (_direction == direction.LEFT && MostLeftCoord() > windowBorder)
            {
                MoveLeft();
            }
            else if (_direction == direction.LEFT && MostLeftCoord() <= windowBorder)
            {
                _direction = direction.DOWN;
            }
            else if (_direction == direction.DOWN)
            {
                MoveDown();
                _direction = direction.RIGHT;
            }
            _buttomYPos = MostLeftDown() + _size;
        }

        public void NewMostRight()
        {
            double max = -1;
            int maxI = 0;
            int maxJ = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_enemys[i, j].X() > max && _enemys[i, j].Alive)
                    {
                        max = _enemys[i, j].X();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostRightEnemySerial = (maxI, maxJ);
        }
        public void NewMostLeft()
        {
            double min = 1000;
            int minI = 0;
            int minJ = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
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
        public void NewMostDown()
        {
            double max = 0;
            int maxI = 0;
            int maxJ = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_enemys[i, j].Y() > max && _enemys[i, j].Alive)
                    {
                        max = _enemys[i, j].Y();
                        maxI = i;
                        maxJ = j;
                    }
                }
            }
            _mostButtomEnemySerial = (maxI, maxJ);
        }

        public int RightEnemyCount(int playerXPos, int playerWidth)
        {
            int count = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_enemys[i, j].Alive && _enemys[i, j].X() > playerXPos + playerWidth / 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public int LeftEnemyCount(int playerXPos, int playerWidth)
        {
            int count = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_enemys[i, j].Alive && _enemys[i, j].X() < playerXPos + playerWidth / 2)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        #endregion

        #region Private Methods
        private void MoveLeft()
        {
            double s;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s - _speed);
                }
            }
        }
        private void MoveRight()
        {
            double s;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    s = _enemys[i, j].X();
                    _enemys[i, j].X(s + _speed);
                }
            }
        }
        private void MoveDown()
        {
            int s;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    s = _enemys[i, j].Y();
                    _enemys[i, j].Y(s + _size);
                }
            }
        }

        private double MostRightCoord()
        {
            int x; int y;
            (x, y) = _mostRightEnemySerial;
            double max = _enemys[x, y].X();
            return max;
        }
        private double MostLeftCoord()
        {
            int x; int y;
            (x, y) = _mostLeftEnemySerial;
            double max = _enemys[x, y].X();
            return (max);
        }
        private int MostLeftDown()
        {
            int x; int y;
            (x, y) = _mostButtomEnemySerial;
            int max = _enemys[x, y].Y() + _size;
            return (max);
        }

        #endregion

    }
}

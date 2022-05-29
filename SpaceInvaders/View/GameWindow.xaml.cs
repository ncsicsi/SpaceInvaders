using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SpaceInvaders.Model;
using System.Diagnostics;

namespace SpaceInvaders.View
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        #region Fields
        Rectangle[,] _enemysRectangles; //enemy teglalapok, amik megjelennek
        Rectangle[] _bulletsRectangles;   //lovedek teglalapok
        Rectangle _enemyBulletRectangle;
        int _enemyRows = 0;
        int _enemyColumns = 0;
        int _enemySize = 0;
        #endregion

        /// Játékból való kilépés eseménye.
        public event KeyEventHandler KeyIsDown_Event;
        public event KeyEventHandler KeyIsUp_Event;

        #region Constructor
        public GameWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();
            NewGame();
        }
        #endregion

        public void NewGame()
        {
            //BulletsRectangleCreated();
        }
        public void RoundOver()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                RemoveBullets();
                RemoveEnemys();
                RemoveEnemyBullet();
            }));
        }


        private void RemoveBullets()
        {
            for (int i = 0; i < _bulletsRectangles.Length; i++)
            {
                //if (_enemyBulletRectangle != null)
                //{
                try
                {
                    GameCanvas.Children.Remove(_bulletsRectangles[i]);
                }
                catch
                {

                }
                //}
            }
        }
        private void RemoveEnemyBullet()
        {
            GameCanvas.Children.Remove(_enemyBulletRectangle);
        }
        private void RemoveEnemys()
        {
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    GameCanvas.Children.Remove(_enemysRectangles[i, j]);
                }
            }
            /*this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (var x in GameCanvas.Children.OfType<Rectangle>())
                {
                    if (x is Rectangle && (string)x.Tag == "enemy")
                    {
                        GameCanvas.Children.Remove(x);
                    }
                }
            }));*/
        }

        #region private methods
        public void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (KeyIsDown_Event != null)
                KeyIsDown_Event(this, e);
        }

        public void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (KeyIsUp_Event != null)
                KeyIsUp_Event(this, e);
        }

        //jatek letrehozasanak esemenye
        public void View_GameCreated(EnemyEventArgs e)
        {
            _enemyRows = e.EnemRows;
            _enemyColumns = e.EnemyColumns;
            _enemySize = e.EnemySize;
            _enemysRectangles = new Rectangle[e.EnemRows, e.EnemyColumns];
            _bulletsRectangles = new Rectangle[e.BulletCount];
            Dispatcher.Invoke(() =>
            {
                makeEnemies(e.EnemyCount, e.Enemys);
            });
        }


        // ido elorehaladtanak esemeny
        public void View_GameAdvanced(GameEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                //enemy bullet
                EnemyBulletUpdate(e.EnemyBullet);
                //bullet move
                BulletsUpdate(e.Bullets);
                //enemy move
                EnemysUpdate(e.Enemies);
            });
        }
        private void EnemyBulletUpdate(Bullet enemyBullet)
        {
            if (enemyBullet.IsNewBullet)
            {
                _enemyBulletRectangle = new Rectangle
                {
                    Tag = "enemyBullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.LightCoral,
                    Stroke = Brushes.Red,
                };
                GameCanvas.Children.Add(_enemyBulletRectangle);
                Canvas.SetTop(_enemyBulletRectangle, enemyBullet.Y);
                Canvas.SetLeft(_enemyBulletRectangle, enemyBullet.X);
            }
            else if (enemyBullet.Alive)
            {
                if (_enemyBulletRectangle != null)
                {
                    Canvas.SetTop(_enemyBulletRectangle, enemyBullet.Y);
                }
            }
            else if (!enemyBullet.Alive)
            {
                GameCanvas.Children.Remove(_enemyBulletRectangle);
            }
        }
        public void BulletsUpdate(Bullet[] bullets)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].Alive && bullets[i].IsNewBullet && _bulletsRectangles[i] == null)
                {
                    _bulletsRectangles[i] = new Rectangle
                    {
                        Tag = "bullet",
                        Height = 20,
                        Width = 5,
                        Fill = Brushes.Yellow,
                        Stroke = Brushes.Red,

                    };
                    GameCanvas.Children.Add(_bulletsRectangles[i]);
                    Canvas.SetTop(_bulletsRectangles[i], bullets[i].Y);
                    Canvas.SetLeft(_bulletsRectangles[i], bullets[i].X);
                }
                else if (bullets[i].Alive && !bullets[i].IsNewBullet && _bulletsRectangles[i] != null)
                {
                    if (bullets[i].Alive && !bullets[i].IsNewBullet && _bulletsRectangles[i] != null)
                        Canvas.SetTop(_bulletsRectangles[i], bullets[i].Y);
                }
                else
                {
                    GameCanvas.Children.Remove(_bulletsRectangles[i]);
                    _bulletsRectangles[i] = null;
                }
            }
        }

        private void EnemysUpdate(EnemyStruct[,] enemies)
        {
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    if (enemies[i, j].Alive && _enemysRectangles[i, j] != null)
                    {

                        Canvas.SetTop(_enemysRectangles[i, j], enemies[i, j].Y());
                        Canvas.SetLeft(_enemysRectangles[i, j], enemies[i, j].X());

                    }
                    else
                    {
                        GameCanvas.Children.Remove(_enemysRectangles[i, j]);
                    }
                }
            }
        }
        private void makeEnemies(int enemiCount, EnemyStruct[,] enemies)
        {
            for (int i = 0; i < _enemyRows; i++)
            {
                for (int j = 0; j < _enemyColumns; j++)
                {
                    Trace.WriteLine("Invoke start" + i + " " + j + " ");

                    ImageBrush enemySkin = new ImageBrush();
                    _enemysRectangles[i, j] = new Rectangle
                    {
                        Tag = "enemy",
                        Height = _enemySize,
                        Width = _enemySize,
                        Fill = enemySkin
                    };
                    Console.WriteLine("Currently modified ids are: " + i + " and " + j);
                    Canvas.SetTop(_enemysRectangles[i, j], enemies[i, j].Y());
                    Canvas.SetLeft(_enemysRectangles[i, j], enemies[i, j].X());
                    GameCanvas.Children.Add(_enemysRectangles[i, j]);
                    if (i == 0)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier1.png"));
                    }
                    else if (i == 1)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier2.png"));
                    }
                    else if (i == 2)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier2.png"));
                    }
                    else if (i == 3)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier3.png"));
                    }
                    else if (i == 4)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier3.png"));
                    }
                }
            }
        }
        #endregion
    }
}


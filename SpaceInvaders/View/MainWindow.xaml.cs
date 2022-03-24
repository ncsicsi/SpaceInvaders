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



namespace SpaceInvaders.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Rectangle[,] _enemysRectangles = new Rectangle[5,10];   //enemy teglalapok, amik megjelennek
        Rectangle[] _bulletsRectangles = new Rectangle[15];   //lovedek teglalapok
        private int time = 0;


        /// Játékból való kilépés eseménye.
        public event KeyEventHandler KeyIsDown_Event;
        public event KeyEventHandler KeyIsUp_Event;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();
            BulletsRectangleCreated();




        }
        #endregion

        #region private methods
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (KeyIsDown_Event != null)
                KeyIsDown_Event(this,e);
            /*if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.Yellow,
                    Stroke = Brushes.Red

                };
                Canvas.SetTop(newBullet, Canvas.GetTop(spaceShip) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(spaceShip) + spaceShip.Width / 2 - 2);
                GameCanvas.Children.Add(newBullet);               
            }*/
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (KeyIsUp_Event != null)
                KeyIsUp_Event(this, e);
        }

        // ido elorehaladtanak esemeny
        public void View_GameAdvanced(GameEventArgs e)
        {
            //enemy bullet

            //bullet move
            BulletsUpdate(e.Bullets);
            //enemy move
            EnemysUpdate(e.Enemies);
        }
        private void BulletsRectangleCreated()
        {
            for (int i = 0; i < 15; i++)
            {
                    _bulletsRectangles[i] = new Rectangle
                    {
                        Tag = "bullet",
                        Height = 20,
                        Width = 5,
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black

                    };
                    GameCanvas.Children.Add(_bulletsRectangles[i]);
            }
        }
        public void BulletsUpdate(Bullet[] bullets)
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].Alive)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        _bulletsRectangles[i].Fill = Brushes.Yellow;
                        _bulletsRectangles[i].Stroke = Brushes.Red;
                        Canvas.SetTop(_bulletsRectangles[i], bullets[i].Y);
                        Canvas.SetLeft(_bulletsRectangles[i], bullets[i].X);
                    }));
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        _bulletsRectangles[i].Fill = Brushes.Black;
                        _bulletsRectangles[i].Stroke = Brushes.Black;
                    }));
                }

            }
        }

        private void EnemysUpdate(EnemyStruct[,] enemies)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (enemies[i,j].Alive())
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            Canvas.SetTop(_enemysRectangles[i,j], enemies[i,j].Y());
                            Canvas.SetLeft(_enemysRectangles[i, j], enemies[i,j].X());
                        }));
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            _enemysRectangles[i,j].Fill = Brushes.Black;
                        }));
                    }
                }
            }
        }
        

        public void View_GameCreated(EnemyEventArgs e)
        {
            makeEnemies(50, e.Enemys);
        } 



        private void makeEnemies(int enemiCount, EnemyStruct[,] enemies)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    ImageBrush enemySkin = new ImageBrush();
                    _enemysRectangles[i, j] = new Rectangle
                    {
                        Tag = "enemy",
                        Height = 45,
                        Width = 45,
                        Fill = enemySkin
                    };
                    Canvas.SetTop(_enemysRectangles[i, j], enemies[i,j].Y());
                    Canvas.SetLeft(_enemysRectangles[i, j], enemies[i,j].X());
                    GameCanvas.Children.Add(_enemysRectangles[i, j]);
                    if (i == 0)
                    {
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier1.png"));
                    }
                    else if( i == 1)
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

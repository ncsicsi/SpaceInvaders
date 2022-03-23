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
        int enemyImages;

        /// Játékból való kilépés eseménye.
        public event KeyEventHandler KeyIsDown_Event;
        public event KeyEventHandler KeyIsUp_Event;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();
            makeEnemies(50);

        }
        #endregion

        #region private methods
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (KeyIsDown_Event != null)
                KeyIsDown_Event(this,e);
            if (e.Key == Key.Space)
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

                
            }
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
            int a = 0;
            //enemy move
        } 

        private void makeEnemies(int enemiCount)
        {
            int enemyRow = 0;
            int left = 560;
            enemyImages = 0;
            for (int i =0; i< enemiCount; i++)
            {

                ImageBrush enemySkin = new ImageBrush();
                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = enemySkin
                };

                switch (i)
                {
                    case 10:
                        enemyRow++;
                        left = 560;
                        break;
                    case 20:
                        enemyRow++;
                        left = 560;
                        break;
                    case 30:
                        enemyRow++;
                        left = 560;
                        break;
                    case 40:
                        enemyRow++;
                        left = 560;
                        break;
                }

                Canvas.SetTop(newEnemy, enemyRow*55 + 10);
                Canvas.SetLeft(newEnemy, left);
                GameCanvas.Children.Add(newEnemy);
                
                left -= 55;

                enemyImages++;


                if (enemyImages < 11)
                {
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier1.png"));
                }
                else if(enemyImages < 21)
                {
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier2.png"));
                } 
                else if(enemyImages < 31)
                {
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier2.png"));
                }
                else if (enemyImages < 41)
                {
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier3.png"));
                }
                else if (enemyImages < 51)
                {
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/invadier3.png"));
                }

            }
        }
        #endregion
    }
}

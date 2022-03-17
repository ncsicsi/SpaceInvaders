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


namespace SpaceInvaders.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// Játékból való kilépés eseménye.
        public event KeyEventHandler KeyIsDown_Event;
        public event KeyEventHandler KeyIsUp_Event;

        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (KeyIsDown_Event != null)
                KeyIsDown_Event(this,e);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (KeyIsUp_Event != null)
                KeyIsUp_Event(this, e);
            if(e.Key == Key.Space)
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
                Canvas.SetLeft(newBullet, Canvas.GetLeft(spaceShip)+spaceShip.Width / 2-2);
                GameCanvas.Children.Add(newBullet); 
            }
        }
    }
}

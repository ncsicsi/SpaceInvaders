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
using SpaceInvaders.Model;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceInvaders.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : UserControl
    {

        private double[] _evolutionParmas = new double[7];
        public event EventHandler<paramEventArgs> Save_Event;
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void saveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            bool allCorrect = false;
            try
            {
                _evolutionParmas[0] = Convert.ToDouble(evolutionTypBox.Text); 
                _evolutionParmas[1] = Convert.ToDouble(mutationBox.Text);
                _evolutionParmas[2] = Convert.ToDouble(scoreFitnessBox.Text);
                _evolutionParmas[3] = Convert.ToDouble(elaspsedTimeFitnessBox.Text);
                _evolutionParmas[4] = Convert.ToDouble(avoidBulletsFitnessBox.Text);
                _evolutionParmas[5] = Convert.ToDouble(usedBulletsFitnessBox.Text);
                _evolutionParmas[6] = Convert.ToDouble(goLeftAndRightFitnessBox.Text);
                allCorrect = true;
            }
            catch (Exception h)
            {
                if(evolutionTypBox.Text !="" || mutationBox.Text != "" || scoreFitnessBox.Text != "" || elaspsedTimeFitnessBox.Text != "" || avoidBulletsFitnessBox.Text != "" || usedBulletsFitnessBox.Text != "" || goLeftAndRightFitnessBox.Text != "")
                    MessageBox.Show("Please number only, and fill all!");
            }
            if (!(_evolutionParmas[0] == 0 || _evolutionParmas[0] == 1))
            {
                MessageBox.Show("Populatipn type can only be 0 or 1!");
                allCorrect = false;
            }
            if (allCorrect)
            {
                if (Save_Event != null)
                    Save_Event(this, new paramEventArgs(_evolutionParmas));
            }
        }
    }
}

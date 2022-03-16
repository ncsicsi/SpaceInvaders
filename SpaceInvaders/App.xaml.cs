using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Threading;
using System.ComponentModel;
using SpaceInvaders;
using SpaceInvaders.Model;
using SpaceInvaders.ViewModel;
using SpaceInvaders.View;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private GameModel _model;
        private GameViewModel _viewModel;
        private MainWindow _view;

        #endregion

        #region Constructor

        // alkalmazas peldanyositasa
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            
        }

        #endregion

        #region App Event Handler

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //model letrehozasa
            _model = new GameModel();
            _model.NewGame();

            //nzetmodel letrehozasa
            _viewModel = new GameViewModel(_model);


            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _view.KeyIsDown_Event += new KeyEventHandler(View_KeyIsDown);
            _view.KeyIsUp_Event += new KeyEventHandler(View_KeyIsUp);

            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        #endregion

        #region View event handlers

        /// Nézet bezárásának eseménykezelője.
        private void View_Closing(object sender, CancelEventArgs e)
        {

            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
            }
        }

        // Gomb lenyomasanak esemenye
        private void View_KeyIsDown(object sender, KeyEventArgs e)
        {
            _viewModel.View_KeyIsDown(e); // ablak bezárása
            
        }
        //gomb felengedsenek esemenye
        private void View_KeyIsUp(object sender, KeyEventArgs e)
        {
            _viewModel.View_KeyIsUp(e); // ablak bezárása
        }

        #endregion


        #region ViewModel Event Handlers

        // Uj jatek inditasanak esemenykezeloje
        private void ViewModel_NewGame(object sender, EventArgs e)
        {

            _model.NewGame();

        }

        // Játékból való kilépés eseménykezelője.
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
        }

        #endregion


        #region Model Event Handler

        //jatekvege esemeny
        private void Model_GameOver(object sender, GameEventArgs e)
        {
            //_model.NewGame();
        }

        #endregion

    }
}

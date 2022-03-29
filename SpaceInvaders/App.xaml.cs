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
            _model.GameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);


            //nzetmodel letrehozasa
            _viewModel = new GameViewModel(_model);
            _viewModel.GameAdvanced += new EventHandler<GameEventArgs>(View_GameAdvanced);
            _viewModel.GameCreated += new EventHandler<EnemyEventArgs>(View_GameCreated);

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _view.KeyIsDown_Event += new KeyEventHandler(View_KeyIsDown);
            _view.KeyIsUp_Event += new KeyEventHandler(View_KeyIsUp);
            _model.NewGame();

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
                _model.stopTimer();
                e.Cancel = true; // töröljük a bezárást
                
            }
        }
        /// Nézet bezárásának eseménykezelője.
        private void View_GameAdvanced(object sender, GameEventArgs e)
        {
            _view.View_GameAdvanced(e); 
        }
        private void View_GameCreated(object sender, EnemyEventArgs e)
        {
            _view.View_GameCreated(e); 
        }

        // Gomb lenyomasanak esemenye
        private void View_KeyIsDown(object sender, KeyEventArgs e)
        {
            _viewModel.View_KeyIsDown(e); 
            
        }
        //gomb felengedsenek esemenye
        private void View_KeyIsUp(object sender, KeyEventArgs e)
        {
            _viewModel.View_KeyIsUp(e);
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
        private void Model_GameOver(object sender, GameOverEventArgs e)
        {
            _model.stopTimer();
            if (e.Win)
            {
                _view.GameOver();
                MessageBox.Show("Új kor kovetkezik", "Game End",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                
                _model.NewRound();
                _view.NewGame();
            }
            else
            {
                if (MessageBox.Show("Game Over! \n New Game?", "GameOver", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    _model.stopTimer();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        _view.Close();
                    }));
                }
                else
                {
                    _view.GameOver();
                    _model.NewGame();
                }
            }
        }
        #endregion
    }
}

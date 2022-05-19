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
using SpaceInvaders.Persistence;



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
        private GameWindow _gameWindow;
        private MenuWindow _menuWindow;
        private SettingsWindow _settingsWindow;

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
            _model = new GameModel(new GameFileDataAccess());
            _view = new MainWindow();
            _gameWindow = new GameWindow();
            _menuWindow = new MenuWindow();
            _settingsWindow = new SettingsWindow();
            _viewModel = new GameViewModel(_model);
            _model.GameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);
            //_model.NetworkLoaded += new EventHandler<GameEventArgs>(Model_NetworkLoaded);


            //nzetmodel letrehozasa
            //_viewModel = new GameViewModel(_model);
            _viewModel.GameAdvanced += new EventHandler<GameEventArgs>(View_GameAdvanced);
            _viewModel.GameCreated += new EventHandler<EnemyEventArgs>(View_GameCreated);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.LoadNetwork += new EventHandler(ViewModel_LoadNetwork);
            _viewModel.SaveNetwork += new EventHandler(ViewModel_SaveNetwork);
            _viewModel.BestPlay += new EventHandler(ViewModel_BestPlay);
            _viewModel.TurnOffView += new EventHandler(ViewModel_TurnOffView);
            _viewModel.TurnOnView += new EventHandler(ViewModel_TurnOnView);
            _viewModel.TurnSimpleEvolution += new EventHandler(ViewModel_TurnSimpleEvolution);
            _viewModel.TurnRedQueenEvolution += new EventHandler(ViewModel_TurnRedQueenEvolution);

            // nézet létrehozása
            //_view = new MainWindow();
            _view.DataContext = _viewModel;
            _gameWindow.DataContext = _viewModel;
            //_menuWindow.DataContext = _viewModel;

            _gameWindow.KeyIsDown_Event += new KeyEventHandler(View_KeyIsDown);
            _gameWindow.KeyIsUp_Event += new KeyEventHandler(View_KeyIsUp);
            //_model.NewGame();
            //_model.OnGameCreated();
            _viewModel.NewGameAIView += (o, args) => { _view.Navigate(_gameWindow); };
            _viewModel.NewGameManualView += (o, args) => { _view.Navigate(_gameWindow); };
            _viewModel.NetworkLoadView += (o, args) => { _view.Navigate(_gameWindow); };
            _viewModel.BackToTheMenu += (o, args) => { _view.Navigate(_menuWindow); };
            _viewModel.SettingsView += (o, args) => { _view.Navigate(_settingsWindow); };

            _gameWindow.KeyIsDown_Event += new KeyEventHandler(View_KeyIsDown);
            _gameWindow.KeyIsUp_Event += new KeyEventHandler(View_KeyIsUp);
            //_model.NewGame();
            _view.Navigate(_menuWindow);
            _view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Navigate(_menuWindow);
            _view.Show();
        }

        #endregion

        #region View event handlers

        /// Nézet bezárásának eseménykezelője.
        private void View_Closing(object sender, CancelEventArgs e)
        {
            _model.stopTimer();
            if (MessageBox.Show("Biztos, hogy ki akar lépni?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; // töröljük a bezárást
                if (_model._startGame)
                {
                    _model.startTimer();
                }
            }
            else
            {
                //_model.stopTimer();
                //_gameWindow.RoundOver();
            }
        }
        /// Nézet bezárásának eseménykezelője.
        private void View_GameAdvanced(object sender, GameEventArgs e)
        {
            _gameWindow.View_GameAdvanced(e); 
        }
        private void View_GameCreated(object sender, EnemyEventArgs e)
        {
            _gameWindow.View_GameCreated(e); 
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
            _gameWindow.RoundOver();
            _model.NewGame();
        }
        //legugyesebb lejatszasa
        private void ViewModel_BestPlay(object sender, EventArgs e)
        {
            _gameWindow.RoundOver();
            _model.NewGame();
            _model.BestPlay();
            _gameWindow.NewGame();
        }
        //nezet ki, be kapcsolasa
        private void ViewModel_TurnOffView(object sender, EventArgs e)
        {

        }       
        private void ViewModel_TurnOnView(object sender, EventArgs e)
        {

        }        
        private void ViewModel_TurnSimpleEvolution(object sender, EventArgs e)
        {
            //_model.stopTimer();
            //_view.RoundOver();
        }        
        private void ViewModel_TurnRedQueenEvolution(object sender, EventArgs e)
        {
            //_model.stopTimer();
            //_view.RoundOver();
        }




        // Játékból való kilépés eseménykezelője.
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            
            _view.Close(); // ablak bezárása
        }

        // network betöltésének eseménykezelője.
        private async void ViewModel_LoadNetwork(object sender, System.EventArgs e)
        {
            /*try
            {*/
                _model.stopTimer();
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "neuralis halok betöltése";
                openFileDialog.Filter = "neuralis halok|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadNetworkAsync(openFileDialog.FileName);
                    _gameWindow.RoundOver();
                    _model.NewGame();
                    _gameWindow.NewGame();

            }
            /*}
            catch (GameDataException)
            {
                MessageBox.Show("A fájl betöltése sikertelen!", "Sudoku", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/

        }

        // neowork mentésének eseménykezelője.
        private async void ViewModel_SaveNetwork(object sender, EventArgs e)
        {

            try
            {
                _model.stopTimer();
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "neuralis hálok mentese";
                saveFileDialog.Filter = "neurális hálók|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    
                    try{
                        // játéktábla mentése
                        await _model.SaveNetworkAsync(saveFileDialog.FileName);
                    }
                    catch (GameDataException)
                    {
                        MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                _model.startTimer();
            }
            catch
            {
                MessageBox.Show("A fájl mentése sikertelen!", "Invaders", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion


        #region Model Event Handler
        private void Model_NetworkLoaded(object sender, GameOverEventArgs e)
        {
            //_view.NewGame();
        }

        //jatekvege esemeny
        private void Model_GameOver(object sender, GameOverEventArgs e)
        {
            _model.stopTimer();
            if (e.Win)
            {
                _gameWindow.RoundOver();
                /*MessageBox.Show("Új kor kovetkezik", "Game End",
                    MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                */
                _model.NewRound();
                _gameWindow.NewGame();
            }
            else if (e.NetworkOn)
            {
                _model.stopTimer();
                _gameWindow.RoundOver();
                _model.NewGame();
            }
            else
            {
                if (MessageBox.Show("Game Over! \n New Game?", "RoundOver", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    _model.stopTimer();
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        _view.Close();
                    }));
                }
                else
                {
                    _gameWindow.RoundOver();
                    _model.NewGame();
                }
            }
        }
        #endregion
    }
}

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
            _viewModel.BackToTheMenu += new EventHandler(ViewModel_BackToTheMenu);

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
            _settingsWindow.Save_Event += new EventHandler<paramEventArgs> (Model_SaveParams);
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
            if (MessageBox.Show("Are you sure about the exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
        private void Model_SaveParams(object sender, paramEventArgs e)
        {
            _model.Model_SaveParams(e);
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
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't play BestPlay in manual mode", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
            else
            {
                _gameWindow.RoundOver();
                _model.NewGame();
                _model.BestPlay();
                _gameWindow.NewGame();
            }
        }
        //nezet ki, be kapcsolasa
        private void ViewModel_TurnOffView(object sender, EventArgs e)
        {
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't turn off view in manual mode","Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }else if (!_model._viewOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't turn off view", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
        }
        private void ViewModel_TurnOnView(object sender, EventArgs e)
        {
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't turn view in manual mode", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
            else if(_model._viewOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't turn on view", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
        }
        private void ViewModel_TurnSimpleEvolution(object sender, EventArgs e)
        {
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't change population in manual mode", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
        }
        private void ViewModel_TurnRedQueenEvolution(object sender, EventArgs e)
        {
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't change population in manual mode", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
        }
        private void ViewModel_BackToTheMenu(object sender, EventArgs e)
        {
            if (_model._startGame)
            {
                _model.stopTimer();
                _model._startGame = false;
                _gameWindow.RoundOver();
            }
        }



        // Játékból való kilépés eseménykezelője.
        private void ViewModel_ExitGame(object sender, System.EventArgs e)
        {
            
            _view.Close(); // ablak bezárása
        }

        // network betöltésének eseménykezelője.
        private async void ViewModel_LoadNetwork(object sender, System.EventArgs e)
        {
            try
            {

                _model.stopTimer();
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "neuralis halok betöltése";
                openFileDialog.Filter = "neuralis halok|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    _viewModel.NavigateLoadNetwork();
                    // játék betöltése
                    await _model.LoadNetworkAsync(openFileDialog.FileName);
                    _gameWindow.RoundOver();
                    _model.NewGame();
                    _gameWindow.NewGame();
                    

                }
                else
                {
                    if (_model._startGame) _model.startTimer();
                }
            }
            catch (GameDataException)
            {
                MessageBox.Show("Population load failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // neowork mentésének eseménykezelője.
        private async void ViewModel_SaveNetwork(object sender, EventArgs e)
        {
            if (!_model.NetworkOn)
            {
                _model.stopTimer();
                MessageBox.Show("Can't save population in manual mode", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                _model.startTimer();
            }
            else
            {
                try
                {
                    _model.stopTimer();
                    SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                    saveFileDialog.Title = "population save";
                    saveFileDialog.Filter = "population|*.stl";
                    if (saveFileDialog.ShowDialog() == true)
                    {

                        try
                        {
                            // játéktábla mentése
                            await _model.SaveNetworkAsync(saveFileDialog.FileName);
                        }
                        catch (GameDataException)
                        {
                            MessageBox.Show("Population save failed!" + Environment.NewLine + "The path is invalid or the directory cannot be written", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    _model.startTimer();
                }
                catch
                {
                    MessageBox.Show("Population save failed!", "Invaders", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

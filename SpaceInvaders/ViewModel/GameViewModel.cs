using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceInvaders.Model;
using System.Windows.Threading; //timer
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpaceInvaders;
using SpaceInvaders.View;


namespace SpaceInvaders.ViewModel
{
    internal class GameViewModel : ViewModelBase
    {
        #region fields
        private GameModel _model;
        private double[] _eolutionParameters;
        #endregion

        #region Properties

        /// Új játék kezdése parancs lekérdezése.
        public DelegateCommand NewGameCommand { get; private set; }

        /// Kilépés parancs lekérdezése.
        public DelegateCommand ExitCommand { get; private set; }

        /// Halo sulyinak betöltése parancs lekérdezése.
        public DelegateCommand LoadNetworkCommand { get; private set; }

        /// halo sulyinak mentése parancs lekérdezése.
        public DelegateCommand SaveNetworkCommand { get; private set; }
        public DelegateCommand BestPlayCommand { get; private set; }
        public DelegateCommand TurnOffViewCommand { get; private set; }
        public DelegateCommand TurnOnViewCommand { get; private set; }
        public DelegateCommand SimpleEvolutionCommand { get; private set; }
        public DelegateCommand RedQueenEvolutionCommand { get; private set; }
        public DelegateCommand ChangeAICommand { get; private set; }
        public DelegateCommand ChangeManualCommand { get; private set; }

        //for navigation
        /// <summary>
        /// command for navigate to new game view
        /// </summary>
        public DelegateCommand NewGameManualViewCommand { get; private set; }
        public DelegateCommand NewGameAIViewCommand { get; private set; }
        public DelegateCommand LoadNetworkMenuCommand { get; private set; }
        public DelegateCommand BackToTheMenuCommand { get; private set; }
        public DelegateCommand SettingsViewCommand { get; private set; }
        public DelegateCommand SaveParametersCommand { get; private set; }

        // Eletek lekerdezese
        public Int32 GameLives { get {return _model.Lives;} }

        // Pontszam lekerdezese
        public Int32 GameScore { get { return _model.Score; } }
        public Int32 GameRounds { get { return _model.Rounds; } }
        public Int32 ActiveIndividual { get { return (_model.ActiveIndividual + 1); } }
        // hajo helyzetenek lekerdezese lekerdezese
        public Int32 XPos { get { return _model.XPos; } }
        #endregion

        #region Events

        /// Új játék eseménye.
        public event EventHandler NewGame;

        /// Játékból való kilépés eseménye.
        public event EventHandler ExitGame;
        public event EventHandler LoadNetwork;
        public event EventHandler SaveNetwork;
        public event EventHandler BestPlay;
        public event EventHandler TurnOffView;
        public event EventHandler TurnOnView;
        public event EventHandler TurnSimpleEvolution;
        public event EventHandler TurnRedQueenEvolution;

        public event EventHandler<GameEventArgs> GameAdvanced;
        public event EventHandler<EnemyEventArgs> GameCreated;
        public event EventHandler<EnemyEventArgs> GameOver;


        // For navigation
        public event EventHandler NewGameAIView;
        public event EventHandler NewGameManualView;
        public event EventHandler NetworkLoadView;
        public event EventHandler BackToTheMenu;
        public event EventHandler SettingsView;


        #endregion

        #region Constructor
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            _model.GameCreated += new EventHandler<EnemyEventArgs>(Model_GameCreated);
            //_model.RoundOver += new EventHandler<GameOverEventArgs>(Model_GameOver);

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadNetworkCommand = new DelegateCommand(param => OnLoadNetwork());
            SaveNetworkCommand = new DelegateCommand(param => OnSaveNetwork());
            BestPlayCommand = new DelegateCommand(param => OnBestPlay());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            TurnOffViewCommand = new DelegateCommand(param => OnTurnOffView());
            TurnOnViewCommand = new DelegateCommand(param => OnTurnOnView());
            SimpleEvolutionCommand = new DelegateCommand(param => OnTurnSimpleEvolution());
            RedQueenEvolutionCommand = new DelegateCommand(param => OnTurnRedQueenEvolution());
            ChangeAICommand = new DelegateCommand(param => OnChangeAI());
            ChangeManualCommand = new DelegateCommand(param => OnChangeManual());
            // Navigalashoz commandok
            NewGameManualViewCommand = new DelegateCommand(param => { OnNewGameManualView(); });
            NewGameAIViewCommand = new DelegateCommand(param => { OnNewGameAIView(); });
            LoadNetworkMenuCommand = new DelegateCommand(param => { OnLoadNetworkMenu(); });
            BackToTheMenuCommand = new DelegateCommand(param => { OnBackToTheMenu(); });
            SettingsViewCommand = new DelegateCommand(param => { OnSettingsView(); });
            
            SetUpTable();
        }
        #endregion

        #region Private methods
        private void SetUpTable()
        {
            OnPropertyChanged("GameLives");
            OnPropertyChanged("GameScore");
            OnPropertyChanged("XPos");
            OnPropertyChanged("GameRounds");
            OnPropertyChanged("ActiveIndividual");
            
        }


        #endregion

        #region Game event handler


        private void Model_GameOver(object sender, GameEventArgs e)
        {
            int a = 1;
        }



        /// Játék előrehaladásának eseménykezelője.
        private void Model_GameAdvanced(object sender, GameEventArgs e)
        {
            OnPropertyChanged("GameLives");
            OnPropertyChanged("GameScore");
            OnPropertyChanged("XPos");
            OnPropertyChanged("GameRounds");
            OnPropertyChanged("ActiveIndividual");
            if (GameAdvanced != null)
                GameAdvanced(this, e);
        } 
        private void Model_GameCreated(object sender, EnemyEventArgs e)
        {
            if (GameCreated != null)
                GameCreated(this, e);
            
        }
        // billentyuzet lenyomasa esemeny
        public void View_KeyIsDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (_model.NetworkOn == false)
                    {
                        _model.GoLeft(true);
                    }
                    break;
                case Key.Right:
                    if (_model.NetworkOn == false)
                    {
                        _model.GoRight(true);
                    }
                    break;
                case Key.Space:
                    if (_model.NetworkOn == false)
                    {
                        //_model.BulletOn(true);
                    }
                    break;
                case Key.M:
                    //_manual = true;
                    //_ai = false;
                    _model.ChangeManual();
                    break;
                case Key.A:
                    //_manual = false;
                    //_ai = true;
                    _model.ChangeAI();
                    break;
            }
        }
        //billentyuzet felengedese esemeny
        public void View_KeyIsUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (_model.NetworkOn == false)
                    {
                        _model.GoLeft(false);
                    }
                    break;
                case Key.Right:
                    if (_model.NetworkOn == false)
                    {
                        _model.GoRight(false);
                    }
                    break;
                case Key.Space:
                    if (_model.NetworkOn == false)
                    {
                        _model.BulletOn(true);
                    }
                    break;
            }
        }

        /// Neuralis halo iranyitasara valtas
        private void OnChangeAI()
        {
            _model.ChangeAI();
        }
        /// Manualis iranyitasara valtas
        private void OnChangeManual()
        {
            _model.ChangeManual();
        }

        /// Játékból való kilépés eseménykiváltása.
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }
        //Uj jatek esemenykivaltasa
        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }
        /// Játék betöltése eseménykiváltása.
        private void OnLoadNetwork()
        {
            if (LoadNetwork != null)
                LoadNetwork(this, EventArgs.Empty);
            // GenerateTabel();
        }

        /// Játék mentése eseménykiváltása.
        private void OnSaveNetwork()
        {
            if (SaveNetwork != null)
                SaveNetwork(this, EventArgs.Empty);
        }        
        // Legjobb halo lejatszasa
        private void OnBestPlay()
        {
            if (BestPlay != null)
                BestPlay(this, EventArgs.Empty);
        }        
        // Grafika ki, be kapcsolas esemenykivaltas
        private void OnTurnOffView()
        {
            if (TurnOffView != null)
            {
                TurnOffView(this, EventArgs.Empty);
                _model.TurnOffView();
            }
        }        
        private void OnTurnOnView()
        {
            if (TurnOnView != null)
            {
                TurnOnView(this, EventArgs.Empty);
                _model.TurnOnView();
            }
        }  
        //Evolucio valtoztatasa
        private void OnTurnSimpleEvolution()
        {
            if (TurnSimpleEvolution != null)
            {
                TurnSimpleEvolution(this, EventArgs.Empty);
                _model.TurnSimpleEvolution();
            }
        }        
        private void OnTurnRedQueenEvolution()
        {
            if (TurnRedQueenEvolution != null)
            {
                TurnRedQueenEvolution(this, EventArgs.Empty);
                _model.TurnRedQueenEvolution();
            }
        }
        private void OnNewGameManualView()
        {
            _model.ChangeManual();
            NewGameManualView.Invoke(this, EventArgs.Empty);
        }        
        private void OnNewGameAIView()
        {
            _model.ChangeAI();
            NewGameAIView.Invoke(this, EventArgs.Empty);
        }
        private void OnLoadNetworkMenu()
        {
            if (LoadNetwork != null) { 
            LoadNetwork(this, EventArgs.Empty);
            //_model.NewGame();
            //NetworkLoadView.Invoke(this, EventArgs.Empty);
        }
        
        }       
        private void OnBackToTheMenu()
        {
            BackToTheMenu.Invoke(this, EventArgs.Empty);
        }
        private void OnSettingsView()
        {
            SettingsView.Invoke(this, EventArgs.Empty);
        }
        public void NavigateLoadNetwork()
        {
            if(!_model._startGame)_model.NewGame();

            NetworkLoadView.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

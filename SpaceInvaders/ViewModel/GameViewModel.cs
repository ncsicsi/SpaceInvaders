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
        private bool _manual = true;
        private bool _ai = false;
        #endregion

        #region Properties

        /// Új játék kezdése parancs lekérdezése.
        public DelegateCommand NewGameCommand { get; private set; }

        /// Kilépés parancs lekérdezése.
        public DelegateCommand ExitCommand { get; private set; }
        
        // Eletek lekerdezese
        public Int32 GameLives { get {return _model.Lives;} }

        // Pontszam lekerdezese
        public Int32 GameScore { get { return _model.Score; } }
        // hajo helyzetenek lekerdezese lekerdezese
        public Int32 XPos { get { return _model.XPos; } }
        #endregion

        #region Events

        /// Új játék eseménye.
        public event EventHandler NewGame;

        /// Játékból való kilépés eseménye.
        public event EventHandler ExitGame;

        public event EventHandler<GameEventArgs> GameAdvanced;
        public event EventHandler<EnemyEventArgs> GameCreated;
        public event EventHandler<EnemyEventArgs> GameOver;
        #endregion

        #region Constructor
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            _model.GameCreated += new EventHandler<EnemyEventArgs>(Model_GameCreated);
            //_model.RoundOver += new EventHandler<GameOverEventArgs>(Model_GameOver);

            SetUpTable();
        }
        #endregion

        #region Private methods
        private void SetUpTable()
        {
            OnPropertyChanged("GameLives");
            OnPropertyChanged("GameScore");
            OnPropertyChanged("XPos");
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

        /// Játékból való kilépés eseménykiváltása.
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        #endregion
    }
}

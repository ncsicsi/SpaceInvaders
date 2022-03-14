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
        DispatcherTimer _timer = new DispatcherTimer();
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
        public Int32 YPos { get { return _model.YPos; } }
        #endregion

        #region Events

        /// Új játék eseménye.
        public event EventHandler NewGame;

        /// Játékból való kilépés eseménye.
        public event EventHandler ExitGame;
        #endregion

        #region Constructor
        public GameViewModel(GameModel model)
        {
            _model = model;
            //_timer.Tick += GameAdvancment;
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Start();




            SetUpTable();
            //GameCanvas.Focus();
        }
        #endregion

        #region Private methods
        private void SetUpTable()
        {
            OnPropertyChanged("GameLives");
            OnPropertyChanged("GameScore");      
        }

        #endregion

        #region Game event handler

        /// Játék végének eseménykezelője.
        private void Model_GameOver(object sender, GameEventArgs e)
        {

        }


        /// Játék előrehaladásának eseménykezelője.
        private void Model_GameAdvanced(object sender, GameEventArgs e)
        {

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

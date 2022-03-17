﻿using System;
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
        #endregion

        #region Constructor
        public GameViewModel(GameModel model)
        {
            _model = model;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);

            Canvas GameCanvas = new Canvas();
            GameCanvas.Focus();


            SetUpTable();
            //GameCanvas.Focus();
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

        /// Játék végének eseménykezelője.
        private void GameTimeEvent(object sender, EventArgs e)
        {

        }

        private void Model_GameOver(object sender, GameEventArgs e)
        {

        }



        /// Játék előrehaladásának eseménykezelője.
        private void Model_GameAdvanced(object sender, GameEventArgs e)
        {
            OnPropertyChanged("GameLives");
            OnPropertyChanged("GameScore");
            OnPropertyChanged("XPos");
        }
        // billentyuzet lenyomasa esemeny
        public void View_KeyIsDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _model.GoLeft(true);
                    break;
                case Key.Right:
                    _model.GoRight(true);
                    break;
                case Key.Enter:
                    break;
            }
        }
        //billentyuzet felengedese esemeny
        public void View_KeyIsUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    _model.GoLeft(false);
                    break;
                case Key.Right:
                    _model.GoRight(false);
                    break;
                case Key.Enter:
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

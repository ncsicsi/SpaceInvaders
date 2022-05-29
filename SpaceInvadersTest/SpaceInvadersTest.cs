using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Threading;
using SpaceInvaders.Model;
using SpaceInvaders.Persistence;
using Moq;
using System.Reflection;

namespace SpaceInvadersTest
{
    [TestClass]
    public class SpaceInvadersTest
    {
        /// <summary>
        /// tesztelni kivant modell
        /// </summary>
        private GameModel _model;
        /// <summary>
        /// adateleres mock-ja
        /// </summary>
        private Mock<IGameDataAccess> _mock;
        private Data _mockData;


        [TestInitialize]
        public void Initialize()
        {

            _mock = new Mock<IGameDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockData));

            _model = new GameModel(_mock.Object);
        }

        /// <summary>
        /// model letrehozasa teszt
        /// </summary>
        [TestMethod]
        public void CreateModelTest()
        {
            Assert.IsTrue(_model._viewOn);
            Assert.IsFalse(_model.NetworkOn);
            Assert.AreEqual(_model.Score, 0);
            Assert.AreEqual(_model.Rounds, 0);
            Assert.IsFalse(_model._startGame);
    }

        [TestMethod]
        public void NewGameTest()
        {
            _model.NewGame();
            Assert.AreEqual(_model.XPos, 312);
            Assert.AreEqual(_model.Score, 0);
            Assert.AreEqual(_model.Rounds, 0);
            Assert.AreEqual(_model.Lives, 1);
        }

        /// <summary>
        /// Jatekmod valtas tesztek
        /// </summary>
        [TestMethod]
        public void ChangeManualModeTest()
        {
            _model.ChangeManual();
            Assert.IsFalse(_model.NetworkOn);
            Assert.AreEqual(_model.ActiveIndividual, -1);
        }

        [TestMethod]
        public void ChangeAIModeTest()
        {
            _model.ChangeAI();
            Assert.IsTrue(_model.NetworkOn);
            Assert.AreNotEqual(_model.ActiveIndividual, -1);
        }

        [TestMethod]
        public void ChangeAIAndBackToManualModeTest()
        {
            _model.ChangeAI();
            Assert.IsTrue(_model.NetworkOn);
            Assert.AreNotEqual(_model.ActiveIndividual, -1); _model.ChangeManual();
            Assert.IsFalse(_model.NetworkOn);
            Assert.AreEqual(_model.ActiveIndividual, -1);
        }

        /// <summary>
        /// nezet ki es bekapcsolasa teszteles
        /// </summary>
        [TestMethod]
        public void TurneOffViewTest()
        {
            _model.ChangeAI();
            _model.TurnOffView();
            Assert.IsFalse(_model._viewOn);
            _model.TurnOffView();
            Assert.IsFalse(_model._viewOn);
        }

        [TestMethod]
        public void CantTurnOffViewManualModeTest()
        {
            _model.ChangeManual();
            _model.TurnOffView();
            Assert.IsTrue(_model._viewOn);
        }
        [TestMethod]
        public void TurneOnViewTest()
        {
            _model.ChangeAI();
            _model.TurnOnView();
            Assert.IsTrue(_model._viewOn);
            _model.TurnOffView();
            Assert.IsFalse(_model._viewOn);
        }

        [TestMethod]
        public void CantTurnOnViewManualModeTest()
        {
            _model.ChangeManual();
            _model.TurnOffView();
            Assert.IsTrue(_model._viewOn);
        }

        /// <summary>
        /// Valtas Voroskiralynora/simara
        /// </summary>
        [TestMethod]
        public void ChangeSimpleEvolutionTest()
        {
            _model.ChangeAI();
            _model.TurnSimpleEvolution();
            Assert.AreEqual(_model.EvolutionType, Network.evolution.SIMPLE);
        }

        [TestMethod]
        public void ChangeRedEvolutionTest()
        {
            _model.ChangeAI();
            _model.TurnRedQueenEvolution();
            Assert.AreEqual(_model.EvolutionType, Network.evolution.REDQUEEN);
        }

        [TestMethod]
        public void ChangeRedAndBackSimpleEvolution()
        {
            _model.ChangeAI();
            _model.TurnSimpleEvolution();
            Assert.AreEqual(_model.EvolutionType, Network.evolution.SIMPLE);
            _model.TurnRedQueenEvolution();
            Assert.AreEqual(_model.EvolutionType, Network.evolution.REDQUEEN);
        }

        /// <summary>
        /// mozgas tesztek
        /// </summary>
        [TestMethod]
        public void GoRightTest()
        {
            _model.ChangeManual();
            _model.NewGame();
            Assert.AreEqual(_model.XPos, 312);
            _model.GoRight(true);
            MethodInfo method = _model.GetType().GetMethod("GameModelAdvanced", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 317);
            _model.XPos = 627;
            Assert.AreEqual(_model.XPos, 627);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 627);

        }

        [TestMethod]
        public void GoLeftTest()
        {
            _model.ChangeManual();
            _model.NewGame();
            Assert.AreEqual(_model.XPos, 312);
            _model.GoLeft(true);
            MethodInfo method = _model.GetType().GetMethod("GameModelAdvanced", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 307);
            _model.XPos = 7;
            Assert.AreEqual(_model.XPos, 7);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 7);
        }

        [TestMethod]
        public void GoLeftAndRgihtTest()
        {
            _model.ChangeManual();
            _model.NewGame();
            Assert.AreEqual(_model.XPos, 312);
            _model.GoLeft(true);
            MethodInfo method = _model.GetType().GetMethod("GameModelAdvanced", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 307);
            _model.GoLeft(false);
            _model.GoRight(true);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 312);
        }


        /// <summar
        /// <summary>
        /// loves teszt
        /// </summary>
        [TestMethod]
        public void NewBulletTest()
        {
            _model.ChangeManual();
            _model.NewGame();
            _model.BulletOn(true);
            MethodInfo method = _model.GetType().GetMethod("GameModelAdvanced", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 312);
        }

        [TestMethod]
        public void AllActionTogetherTest()
        {
            _model.ChangeManual();
            _model.NewGame();
            _model.BulletOn(true);
            MethodInfo method = _model.GetType().GetMethod("GameModelAdvanced", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 312);
            _model.GoRight(true);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 317);
            _model.GoRight(false);
            _model.GoLeft(true);
            method.Invoke(_model, null);
            Assert.AreEqual(_model.XPos, 312);
        }

        /// <summary>
        /// jatek vege teszt
        /// </summary>
        [TestMethod]
        public void GameOverTest1()
        {
            _model.NewGame();
            Assert.IsFalse(_model.IsGameOver);
            _model.Lives--;
            Assert.IsTrue(_model.IsGameOver);
        }

    }

}

using Minefield;
using Minefield.Model;
using Minefield.Persistence;
using Minefield.ViewModel;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using System.Windows.Input;

namespace Minefield

{
    /// <summary>
    /// Alkalmazás típusa.
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MinefieldGameModel _model;
        private MinefieldViewModel _viewModel;
        private MainWindow _view;
        private DispatcherTimer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Alkalmazás példányosítása.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object sender, StartupEventArgs e)
        { 
            // modell létrehozása
            _model = new MinefieldGameModel(new MinefieldFileDataAccess());
            _model.GameOver += new EventHandler<EventArgs>(Model_GameOver);
            _model.NewGame();

            // nézemodell létrehozása
            _viewModel = new MinefieldViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
            _viewModel.ContinueGame += new EventHandler(ViewModel_ContinueGame);
            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();

            _view.KeyDown += new KeyEventHandler(_viewModel.Key_Down);

            // időzítő létrehozása
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _model.NewGame();
            _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object sender, System.EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Loading minefield table";
                openFileDialog.Filter = "Minefield table|*.mftl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    await _model.LoadGameAsync(openFileDialog.FileName);

                    _timer.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Loading was not successful!", "Minefield", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.IsEnabled;

            _timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Loading minefield table";
                saveFileDialog.Filter = "Minefield table|*.mftl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Saving was not successful!" + Environment.NewLine + "Wrong path or permission error.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Saving was not successful!", "Minefield", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();
        }


        private void ViewModel_PauseGame(object sender, EventArgs e)
        {
            _timer.Stop();
            _model.Pause();
            MessageBox.Show("Paused!", "Minefield game");

        }
        private void ViewModel_ContinueGame(object sender, EventArgs e)
        {
            _timer.Start();
            _model.Continue();

        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, EventArgs e)
        {
            _timer.Stop();

                MessageBox.Show("Game Over!",
                                "Minefield game",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            
        }



        #endregion
    }
}

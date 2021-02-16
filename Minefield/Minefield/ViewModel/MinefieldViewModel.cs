using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minefield.Model;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;



namespace Minefield.ViewModel
{
    public class MinefieldViewModel : ViewModelBase
    {
        #region Fields

        private MinefieldGameModel _model; // modell

        #endregion

        #region Properties

        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }


        public DelegateCommand PauseCommand { get; private set; }

        public DelegateCommand ContinueCommand { get; private set; }

        public ObservableCollection<Field> Fields { get; set; }


        /// <summary>
        /// Fennmaradt játékidő lekérdezése.
        /// </summary>
        public String GameTime { get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); } }

        #endregion

        #region Events

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler NewGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;


        public event EventHandler PauseGame;

        public event EventHandler ContinueGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public MinefieldViewModel(MinefieldGameModel model)
        {
            // játék csatlakoztatása
            _model = model;
            _model.GameAdvanced += new EventHandler<EventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<EventArgs>(Model_GameOver);
           

            // parancsok kezelése
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            PauseCommand = new DelegateCommand(param => OnPause());
            ContinueCommand = new DelegateCommand(param => OnContinue());

            // játéktábla létrehozása
            Fields = new ObservableCollection<Field>();
            for (Int32 i = 0; i < 10; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 10; j++)
                {
                    Fields.Add(new Field
                    {
                        Type =Persistence.FieldType.Empty,
                        X = i,
                        Y = j,
                        Number = i * 10 + j
                    });
                    
                }
            }
            Fields.Last().IsPlayer = true;
            RefreshTable();

        }

        #endregion

        private void RefreshTable()
        {
            foreach (Field field in Fields) // inicializálni kell a mezőket is
            {
                field.Type = _model.Table.fieldValues[field.X, field.Y];
            }
        }


        #region Key events

        /// <summary>
        /// Irányítás
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Key_Down(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.D)
            {
                _model.Step(Dir.Right);
            }
            else if (e.Key == Key.S)
            {
                _model.Step(Dir.Down);
            }
            else if (e.Key == Key.A)
            {
                _model.Step(Dir.Left);
            }
            else if (e.Key == Key.W)
            {
                _model.Step(Dir.Up);
            }
        }
        #endregion


        #region Game event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object sender, EventArgs e)
        {   
            RefreshTable();
            OnPropertyChanged("GameTime");
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }



        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnPause()
        {
            if (PauseGame != null)
                PauseGame(this, EventArgs.Empty);
        }

        private void OnContinue()
        {
            if (ContinueGame != null)
                ContinueGame(this, EventArgs.Empty);
        }

        #endregion

    }
}

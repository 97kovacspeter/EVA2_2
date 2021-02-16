using System;

namespace Minefield.ViewModel
{
    public class Field : ViewModelBase
    {

        private Minefield.Persistence.FieldType type;

        public bool IsPlayer { get { return type == Persistence.FieldType.Player; } set { type= value ? Persistence.FieldType.Player : Persistence.FieldType.Empty; } }

        public bool IsLight { get { return type == Persistence.FieldType.LightB; }  }

        public bool IsMedium { get { return type == Persistence.FieldType.MediumB; }  }

        public bool IsHeavy { get { return type == Persistence.FieldType.HeavyB; }   }

        public bool IsEmpty { get { return type == Persistence.FieldType.Empty; }  }

        public Minefield.Persistence.FieldType Type { get { return type; } set { type = value;
                OnPropertyChanged("IsPlayer");
                OnPropertyChanged("IsLight");
                OnPropertyChanged("IsMedium");
                OnPropertyChanged("IsHeavy");
                OnPropertyChanged("IsEmpty");
            } }


        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Sorszám lekérdezése.
        /// </summary>
        public Int32 Number { get; set; }

        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }
    }
}

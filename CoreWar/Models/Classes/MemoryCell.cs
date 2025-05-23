using System.ComponentModel;

namespace CoreWar {
    /// <summary>
    /// A memória egy celláját reprezentáló osztály
    /// </summary>
    public class MemoryCell : INotifyPropertyChanged {
        /// <summary>
        /// A cella sorszáma a memóriában
        /// </summary>
        public int Index { get; private set; }

        private Instruction _instruction;

        /// <summary>
        /// A memóriában tárolt utasítás
        /// </summary>
        public Instruction Instruction {
            get => _instruction;
            set {
                _instruction = value;
                OnPropertyChanged(nameof(Instruction));
            }
        }

        private string _lastModifiedBy;

        /// <summary>
        /// Annak a játékosnak a neve, aki legutóbb módosította a cellát
        /// </summary>
        public string LastModifiedBy {
            get => _lastModifiedBy;
            set {
                _lastModifiedBy = value;
                OnPropertyChanged(nameof(LastModifiedBy));
            }
        }

        public MemoryCell(int index, Instruction instruction) {
            Index = index;
            Instruction = instruction;
            LastModifiedBy = "senki";
        }

        public override string ToString() {
            return $"#{Index}:\n{Instruction}\nLegutóbb módosította: {LastModifiedBy}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// A GUI frissítéséért felelős metódus
        /// </summary>
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

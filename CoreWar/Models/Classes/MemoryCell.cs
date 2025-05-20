using System.ComponentModel;

namespace CoreWar {
    public class MemoryCell : INotifyPropertyChanged {
        public int Index { get; private set; }

        private Instruction _instruction;
        public Instruction Instruction {
            get => _instruction;
            set {
                _instruction = value;
                OnPropertyChanged(nameof(Instruction));
            }
        }

        private string _lastModifiedBy;

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
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}

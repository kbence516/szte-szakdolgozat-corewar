using CoreWar;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;


namespace CoreWarGUI {
    public sealed partial class VMSettingsPage : Page {
        private readonly string logo;

        public VMSettingsPage() {
            InitializeComponent();
            logo = Utils.GetLogo();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(MainPage));
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if (double.IsNaN(MemorySizeNumberBox.Value) || double.IsNaN(MaxCyclesNumberBox.Value) || double.IsNaN(WarriorsNumberBox.Value)|| double.IsNaN(MaxProcessesNumberBox.Value)) {
                ErrorText.Title = "Egyik mezõ sem maradhat üresen!";
                ErrorText.IsOpen = true;
                return;
            }
            int memorySize = Convert.ToInt32(MemorySizeNumberBox.Value);
            int maxCycles = Convert.ToInt32(MaxCyclesNumberBox.Value);
            int warriors = Convert.ToInt32(WarriorsNumberBox.Value);
            int maxProcesses = Convert.ToInt32(MaxProcessesNumberBox.Value);

            VM.ResetInstance();
            VM.GetInstance(memorySize, maxCycles, warriors, maxProcesses);

            Frame.Navigate(typeof(PlayerLoaderPage));
        }
    }
}

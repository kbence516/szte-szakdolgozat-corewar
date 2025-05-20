using CoreWar;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI;

namespace CoreWarGUI {
    public sealed partial class CombatPage : Page {
        private VM vm;

        private bool isPlaying;
        private ObservableCollection<VM> VMObservable { get; } = new();

        public CombatPage() {
            InitializeComponent();
            vm = VM.GetInstance();
            VMObservable.Add(vm);
            isPlaying = false;
        }

        public static List<Color> ColorsList = [Colors.Green, Colors.Yellow, Colors.Purple, Colors.Blue, Colors.Orange, Colors.DarkGreen, Colors.Turquoise, Colors.Pink];

        private async void PlayButton_Click(object sender, RoutedEventArgs e) {
            isPlaying = !isPlaying;
            NextStepButton.IsEnabled = !NextStepButton.IsEnabled;
            if (isPlaying) {
                (sender as Button)!.Content = "Szünet";
            } else {
                (sender as Button)!.Content = "Indítás";
            }

            do {
                PlayACycle();
                await Task.Delay(1);
            } while (isPlaying);
            (sender as Button)!.Content = "Indítás";
            NextStepButton.IsEnabled = !NextStepButton.IsEnabled;
        }

        private async void PlayACycle() {
            string nextLoser = vm.Play();
            if (!nextLoser.Equals("")) {
                isPlaying = false;
                ContentDialog dialog = new();
                dialog.XamlRoot = Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = nextLoser + " vesztett!";
                dialog.DefaultButton = ContentDialogButton.Primary;
                if (vm.Players.Count > 1) {
                    dialog.PrimaryButtonText = "Játék folytatása";
                    dialog.Content = new TextBlock {
                        Text = vm.Players.Count + " játékos maradt.",
                        TextWrapping = TextWrapping.Wrap
                    };
                    await dialog.ShowAsync();
                } else {
                    dialog.PrimaryButtonText = "OK";
                    dialog.Content = new TextBlock {
                        Text = "Vége a játéknak, " + vm.Players.Peek().Name + " nyert!",
                        TextWrapping = TextWrapping.Wrap
                    };
                    await dialog.ShowAsync();
                    PlayButton.Visibility = Visibility.Collapsed;
                    NextStepButton.Visibility = Visibility.Collapsed;
                    EndGameButton.Visibility = Visibility.Visible;
                }
            } else if (vm.Cycle == vm.MaxCycles && vm.Players.Count > 1) {
                isPlaying = false;
                ContentDialog dialog = new();
                dialog.XamlRoot = Content.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Vége a játéknak!";
                dialog.DefaultButton = ContentDialogButton.Primary;
                dialog.PrimaryButtonText = "OK";
                dialog.Content = new TextBlock {
                    Text = "A lejátszható körök száma elérte a maximumot, a játék döntetlen.",
                    TextWrapping = TextWrapping.Wrap
                };
                await dialog.ShowAsync();
                PlayButton.Visibility = Visibility.Collapsed;
                NextStepButton.Visibility = Visibility.Collapsed;
                EndGameButton.Visibility = Visibility.Visible;
            }
        }

        private void NextStepButton_Click(object sender, RoutedEventArgs e) {
            PlayACycle();
        }

        private void EndGameButton_Click(object sender, RoutedEventArgs e) {
            VM.ResetInstance();
            Frame.Navigate(typeof(MainPage));
        }
    }
    public partial class PlayerToColorBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int index = VM.GetInstance().OriginalPlayers.IndexOf(value as string);
            if (index >= 0) {
                return new SolidColorBrush(CombatPage.ColorsList[index]);
            } else {
                return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}

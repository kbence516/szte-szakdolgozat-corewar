using CoreWar;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace CoreWarGUI {
    /// <summary>
    /// A fõoldal XAML kódja mögötti logika
    /// </summary>
    public sealed partial class MainPage : Page {
        private readonly string logo;
        public MainPage() {
            InitializeComponent();
            logo = Utils.GetLogo();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(VMSettingsPage));
        }

        private async void CreditsButton_Click(object sender, RoutedEventArgs e) {
            ContentDialog dialog = new();
            dialog.XamlRoot = Content.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Névjegy";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.PrimaryButtonText = "OK";
            dialog.Content = new TextBlock {
                Text = Utils.GetCredits(),
                TextWrapping = TextWrapping.Wrap,
            };
            await dialog.ShowAsync();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            Environment.Exit(0);
        }
    }
}

using CoreWar;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoreWarGUI {
    public sealed partial class PlayerLoaderPage : Page {
        private readonly VM vm;
        private readonly int nextPlayerCount;
        private readonly string logo;
        private string? filePath;

        public PlayerLoaderPage() {
            InitializeComponent();
            logo = Utils.GetLogo();
            vm = VM.GetInstance();
            nextPlayerCount = vm.Players.Count + 1;
        }

        private string NameTextBoxHeader {
            get => $"{nextPlayerCount}. játékos neve:";
        }

        private string ButtonText {
            get {
                if (nextPlayerCount < vm.Warriors) {
                    return "Következõ játékos";
                } else {
                    return "Játék indítása";
                }
            }
        }

        private void CPUDecides_Checked(object sender, RoutedEventArgs e) {
            OpenFileButton.Visibility = Visibility.Collapsed;
            RedcodeTextBox.Visibility = Visibility.Collapsed;
            ErrorText.IsOpen = false;
            PickedFileTextBlock.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = true;
        }

        private void ReadFromFile_Checked(object sender, RoutedEventArgs e) {
            OpenFileButton.Visibility = Visibility.Visible;
            RedcodeTextBox.Visibility = Visibility.Collapsed;
            ErrorText.IsOpen = false;
            PickedFileTextBlock.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = false;
        }

        private void ReadFromInput_Checked(object sender, RoutedEventArgs e) {
            OpenFileButton.Visibility = Visibility.Collapsed;
            RedcodeTextBox.Visibility = Visibility.Visible;
            ErrorText.IsOpen = false;
            PickedFileTextBlock.Visibility = Visibility.Collapsed;
            NextButton.IsEnabled = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(VMSettingsPage));
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e) {
            if (NameTextBox.Text.Equals("")) {
                ErrorText.Title = "A játékosnév nem maradhat üresen!";
                ErrorText.IsOpen = true;
                return;
            }
            if (vm.Players.ToArray().Select(p => p.Name).Contains(NameTextBox.Text)) {
                ErrorText.Title = "Ez a játékosnév már foglalt!";
                ErrorText.IsOpen = true;
                return;
            }

            try {
                int firstProcessStart = -1;
                if (CPUDecides.IsChecked == true) {
                    string warriorsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\..\Assets\CommonWarriors");
                    var files = Directory.GetFiles(warriorsPath);
                    //string randomPath = Path.Combine(warriorsPath, @".\scannerY.red");
                    string randomPath = files[new Random().Next(files.Length)];
                    firstProcessStart = RedcodeInputLoader.LoadFromFile(randomPath, NameTextBox.Text);
                } else if (ReadFromFile.IsChecked == true) {
                    firstProcessStart = RedcodeInputLoader.LoadFromFile(filePath!, NameTextBox.Text);
                } else if (ReadFromInput.IsChecked == true) {
                    firstProcessStart = RedcodeInputLoader.LoadFromInput(RedcodeTextBox.Text.ReplaceLineEndings(), NameTextBox.Text);
                }
                Player p = new(NameTextBox.Text, firstProcessStart, vm.MaxProcesses);

                if (vm.Players.Count == vm.Warriors) {
                    WaitPopup.IsOpen = true;
                    await Task.Delay(100);
                    vm.OriginalPlayers = vm.Players.Select(p => p.Name).ToList();
                    Frame.Navigate(typeof(CombatPage));
                } else {
                    Frame.Navigate(typeof(PlayerLoaderPage));
                }

            } catch (NullReferenceException) {
                ErrorText.Title = "Helytelen Redcode formátum!";
                ErrorText.IsOpen = true;
                return;
            }


        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e) {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            senderButton!.IsEnabled = false;

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add(".red");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                if (!file.FileType.ToLower().Equals(".red")) {
                    ErrorText.Title = "Helytelen fájlformátum!";
                    ErrorText.IsOpen = true;
                    NextButton.IsEnabled = false;
                } else {
                    filePath = file.Path;
                    PickedFileTextBlock.Text = "A kiválasztott fájl: " + file.Name;
                    PickedFileTextBlock.Visibility = Visibility.Visible;
                    NextButton.IsEnabled = true;
                }
            }
            //re-enable the button
            senderButton.IsEnabled = true;
        }
    }
}

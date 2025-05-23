using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace CoreWarGUI {
    /// <summary>
    /// A f�ablak XAML k�dja m�g�tti logika
    /// </summary>
    public sealed partial class MainWindow : Window {

        public MainWindow() {
            AppWindow.Resize(new Windows.Graphics.SizeInt32(753, 750));
            var presenter = AppWindow.Presenter as OverlappedPresenter;
            presenter!.IsResizable = false;
            presenter!.IsMaximizable = false;
            ExtendsContentIntoTitleBar = true;
            InitializeComponent();
            RootFrame.Navigate(typeof(MainPage));
        }
    }
}

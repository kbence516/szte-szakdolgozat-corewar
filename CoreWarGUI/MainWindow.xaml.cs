using CoreWar;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CoreWarGUI {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
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

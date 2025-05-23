using Microsoft.UI.Xaml;
using System;

namespace CoreWarGUI {
    public partial class App : Application {
        public static MainWindow MainWindow = new();

        private Window? _window;

        public App() {
            Environment.SetEnvironmentVariable("MICROSOFT_WINDOWSAPPRUNTIME_BASE_DIRECTORY", AppContext.BaseDirectory);
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args) {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}

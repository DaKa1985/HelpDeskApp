using HelpDeskApp.Views;
using HelpDeskApp.ViewModels;
using System.Windows;

namespace HelpDeskApp
{
    public partial class MainWindow : Window
    {
        public MainWindow( bool skipLogin = false)
        {
            InitializeComponent();

            if (skipLogin)
            {
                // Nach Logout – kein Login nötig!
                DataContext = new DashboardViewModel();
                return;
            }

            // Login beim ersten Start:
            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            // Login abgebrochen → App schließen:
            if (result != true)
            {
                Application.Current.Shutdown();
                return;
            }

            // Login erfolgreich → Dashboard laden:
            DataContext = new DashboardViewModel();
        }
    }
}
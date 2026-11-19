
using HelpDeskApp.ViewModels;
using System.Windows;


namespace HelpDeskApp.Views
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            // Login erfolgreich → Fenster schließen:
            _viewModel.LoginSuccessful += () =>
            {
                try
                {
                    // Prüfen, ob es beim ersten Login funktioniert
                    DialogResult = true;  
                }
                catch
                {
                    // Nach Logout kommt kein Dialog, daher ignorieren wir die Exception, und öffnen das Dashboard ohne Login:
                    var mainWindow = new MainWindow(skipLogin: true);
                    System.Windows.Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                }

                Close();
            };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Passwort sicher aus PasswordBox lesen:
            _viewModel.Password = PasswordInput.Password;
            _viewModel.Login();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordInput.Password;
            _viewModel.Register();

            // Passwort nach Registrierung zurücksetzen:
            PasswordInput.Password = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
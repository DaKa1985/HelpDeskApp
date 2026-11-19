using HelpDeskApp.ViewModels;
using System.Windows;

namespace HelpDeskApp.Views
{
    public partial class UserManagementWindow : Window
    {
        public UserManagementWindow()
        {
            InitializeComponent();
            DataContext = new UserManagementViewModel();
        }

        private void SchliessenButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
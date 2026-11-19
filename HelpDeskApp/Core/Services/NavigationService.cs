using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskApp.Views;
using System.Windows;

namespace HelpDeskApp.Core.Services
{
    public class NavigationService : INavigationService
    {
        public void ShowReport(string reportText)
        {
            var window = new ReportWindow(reportText);
            window.ShowDialog();
        }

        public void ShowLogin()
        {
            var window = new LoginWindow();
            window.ShowDialog();
        }

        public bool Confirm(string message, string title = "Bestätigung")
        {
            var result = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
    }
}
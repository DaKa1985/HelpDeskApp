using HelpDeskApp.Models;
using HelpDeskApp.ViewModels;
using System.Windows;

namespace HelpDeskApp.Views
{
    public partial class TicketDetailWindow : Window
    {
        public TicketDetailWindow(ITicket ticket)
        {
            InitializeComponent();
            var viewModel = new TicketDetailViewModel(ticket);
            DataContext = viewModel;

            viewModel.SaveSuccessful += () =>
            {
                DialogResult = true;
                Close();
            };
        }

        private void SchliessenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
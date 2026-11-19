using HelpDeskApp.ViewModels;
using System.Windows;

namespace HelpDeskApp.Views
{
    public partial class TemplateWindow : Window
    {
        public TemplateWindow()
        {
            InitializeComponent();
            var viewModel = new TemplateViewModel();
            DataContext = viewModel;

            viewModel.SaveSuccessful += () =>
            {
                DialogResult = true;
                Close();
            };
        }

        private void AbbrechenButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

using HelpDeskApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HelpDeskApp.Views
{
    /// <summary>
    /// Interaktionslogik für ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private readonly string _reportText;
        public ReportWindow(string reportText)
        {
            InitializeComponent();
            _reportText = reportText;
            ReportTextBox.Text = reportText;
        }

        private void SchliessenButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PdfExportButton_Click(object sender, RoutedEventArgs e)
        {
            PdfExporter.Export(_reportText);
        }
    }
}

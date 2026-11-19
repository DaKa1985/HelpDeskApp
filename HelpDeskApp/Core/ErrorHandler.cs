using System;
using System.Windows;

namespace HelpDeskApp.Core
{
    public static class ErrorHandler
    {
        public static void Handle(Exception ex, string context = "")
        {
            // Log in Konsole:
            System.Diagnostics.Debug.WriteLine(
                $"FEHLER in {context}: {ex.Message}");

            // Benutzer informieren:
            MessageBox.Show(
                $"Ein Fehler ist aufgetreten:\n\n{ex.Message}",
                "Fehler",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void HandleSilent(Exception ex, string context = "")
        {
            // Nur loggen – kein Popup:
            System.Diagnostics.Debug.WriteLine(
                $"FEHLER in {context}: {ex.Message}");
        }
    }
}
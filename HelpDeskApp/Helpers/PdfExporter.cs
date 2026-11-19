using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Windows;

namespace HelpDeskApp.Helpers
{
    public class PdfExporter
    {
        public static void Export(string reportText)
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "Tagesbericht speichern";
            dialog.Filter = "PDF Dateien (*.pdf)|*.pdf";
            dialog.FileName = "Tagesbericht_" +
                              DateTime.Now.ToString("dd_MM_yyyy");

            if (dialog.ShowDialog() != true) return;

            try
            {
                var document = new PdfDocument();
                document.Info.Title = "HelpDesk Tagesbericht";

                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                var titelFont = new XFont("Arial", 18, XFontStyle.Bold);
                var textFont = new XFont("Arial", 12, XFontStyle.Regular);

                double y = 50;
                double x = 50;
                double zeilenAbstand = 20;

                // Titel:
                gfx.DrawString("HelpDesk Tagesbericht",
                    titelFont, XBrushes.DarkBlue, x, y);
                y += zeilenAbstand * 2;

                // Inhalt:
                foreach (var zeile in reportText.Split('\n'))
                {
                    if (string.IsNullOrEmpty(zeile))
                    {
                        y += zeilenAbstand / 2;
                        continue;
                    }

                    // Neue Seite wenn nötig:
                    if (y > page.Height - 50)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        y = 50;
                    }

                    gfx.DrawString(zeile, textFont,
                        XBrushes.Black, x, y);
                    y += zeilenAbstand;
                }

                document.Save(dialog.FileName);
                MessageBox.Show("PDF erfolgreich erstellt!", "Export");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message, "PDF Fehler");
            }
        }
    }
}
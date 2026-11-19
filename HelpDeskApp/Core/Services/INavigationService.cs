using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Core.Services
{
    public interface INavigationService
    {
        // Fenster öffnen:
        void ShowReport(string reportText);
        void ShowLogin();

        // Dialog öffnen:
        bool Confirm(string message, string title = "Bestätigung");
    }
}
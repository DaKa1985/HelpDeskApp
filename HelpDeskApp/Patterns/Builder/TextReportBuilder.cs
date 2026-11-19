using HelpDeskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HelpDeskApp.Patterns.Builder
{
    public class TextReportBuilder : IReportBuilder
    {
        private SupportReport _report = new SupportReport();

        public void AddHeader(string date)
        {
            _report.Header = "Tagesbericht – " + date;
        }

        public void AddTicketList(List<ITicket> tickets)
        {
            foreach (var ticket in tickets)
            {
                // Aus ticket-Objekt lesen und als Text speichern:
                _report.TicketLines.Add("Priorität: " + ticket.Priority + " | " + ticket.Status + 
                    "\n" + ticket.CreatedAt.ToString("dd.MM.yyyy HH:mm") + " |  Erstellt von: " + ticket.CreatedByUsername + 
                    "\nTitel:" + ticket.Title + 
                    "\n\nProblembeschreibung:\n"
                    + ticket.Description + 
                    "\n\nOrt: " + ticket.Location + "" +
                    "\n-----------------------------------------\n");
            }
        }
        public void AddStatistics(List<ITicket> tickets)
        {
            var offen = tickets.Count(t => t.Status == "Offen");
            var inBearbeitung = tickets.Count(t => t.Status == "InBearbeitung");
            var geloest = tickets.Count(t => t.Status == "Geloest");
            var geschlossen = tickets.Count(t => t.Status == "Geschlossen");

            _report.Statistics =
                "Gesamt:          " + tickets.Count +
                "\nOffen:           " + offen +
                "\nIn Bearbeitung:  " + inBearbeitung +
                "\nGelöst:          " + geloest +
                "\nGeschlossen:     " + geschlossen;
        }

        public SupportReport GetResult() => _report;
    }
}

using HelpDeskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Builder
{
    public class ReportDirector
    {
        public void BuildDailyReport(IReportBuilder b, List<ITicket> tickets)
        {
            b.AddHeader(DateTime.Now.ToString("dd.MM.yyyy"));
            b.AddTicketList(tickets);
            b.AddStatistics(tickets);
        }
    }
}

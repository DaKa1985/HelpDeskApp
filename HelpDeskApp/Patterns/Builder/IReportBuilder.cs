using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskApp.Models;

namespace HelpDeskApp.Patterns.Builder
{
    public interface IReportBuilder
    {
        void AddHeader(string date);
        void AddTicketList(List<ITicket> tickets);
        void AddStatistics(List<ITicket> tickets);
        SupportReport GetResult();
    }
}

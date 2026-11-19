using HelpDeskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace HelpDeskApp.Patterns.AbstractFactory
{
    public class CriticalTicketFactory : ITicketFactory
    {
        public ITicket CreateTicket(string description)
        {
            return new CriticalTicket { Description = description };
        }
        public IEscalationAction CreateEscalation() => new AutoEscalation();

    }
}

using HelpDeskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.AbstractFactory
{
    public class NormalTicketFactory : ITicketFactory
    {
        public ITicket CreateTicket(string description)
        {
            return new NormalTicket { Description = description };
        }
        public IEscalationAction CreateEscalation() => new ManualEscalation();

    }
}

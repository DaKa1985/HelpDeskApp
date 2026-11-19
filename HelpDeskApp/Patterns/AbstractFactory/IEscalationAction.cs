using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskApp.Models;
using HelpDeskApp.Patterns.AbstractFactory;

namespace HelpDeskApp.Patterns.AbstractFactory
{
    public interface IEscalationAction
    {
        void Escalate(ITicket ticket);
    }
}

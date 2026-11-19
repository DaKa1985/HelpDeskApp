using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskApp.Models;

namespace HelpDeskApp.Patterns.AbstractFactory
{
    public class AutoEscalation : IEscalationAction
    {
        public void Escalate(ITicket t) 
        { 
            //Sofort an Teamleiter
        }
    }
}

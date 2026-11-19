using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Factory
{
    public class EmailNotification : INotification
    {
        public void Send(string message) 
        { 
            // enter email-logic here...
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Factory
{
    public class ConsoleNotification : INotification
    {
        public void Send(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}

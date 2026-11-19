using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Factory
{
    public class ConsoleCreator : NotificationCreator
    {
        public override INotification CreateNotification()
        {
            return new ConsoleNotification();
        }
    }
}

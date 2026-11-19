using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Factory
{
    public abstract class NotificationCreator
    {
        public abstract INotification CreateNotification();

        public void Notify(string message)
        {
            INotification n = CreateNotification();
            n.Send(message);
        }
    }
}

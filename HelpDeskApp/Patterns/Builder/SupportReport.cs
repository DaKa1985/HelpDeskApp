using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Builder
{
    public class SupportReport
    {
        public string Header { get; set; }
        public List<string> TicketLines { get; set; } = new List<string>();
        public string Statistics { get; set; }
    }
}

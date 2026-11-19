using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class NormalTicket : ITicket
    {
        public string Priority => "NORMAL";
        public string Description { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUsername { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public string Status { get; set; } = "Offen";
        public string InternalNote { get; set; }
        public int? AssignedToUserId { get; set; }
    }
}

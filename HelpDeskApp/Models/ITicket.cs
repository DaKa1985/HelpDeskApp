using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public interface ITicket
    {
        string Priority { get; }
        string Description { get; set; }
        string Title { get; set; }
        string Location { get; set; }
        DateTime CreatedAt { get; set; }
        string CreatedByUsername { get; set; }
        List<string> Attachments { get; set; }
        string Status { get; set; }
        string InternalNote { get; set; }
        int? AssignedToUserId { get; set; }
    }
}

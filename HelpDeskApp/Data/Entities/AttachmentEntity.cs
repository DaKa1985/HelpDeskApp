using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Data.Entities
{
    public class AttachmentEntity
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }

        // Navigation Property zurück zum Ticket:
        public virtual TicketEntity Ticket { get; set; }
    }
}

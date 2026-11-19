using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Data.Entities
{
    public class TicketEntity
    {
        public int Id { get; set; }
        public string Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }        
        public string Status { get; set; } = "Offen";  
        public string InternalNote { get; set; }        
        public int CreatedByUserId { get; set; }        
        public int? AssignedToUserId { get; set; }      

        public virtual ICollection<AttachmentEntity> Attachments { get; set; }
            = new List<AttachmentEntity>();
    }
}

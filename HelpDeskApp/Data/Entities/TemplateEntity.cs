using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Data.Entities
{
    public class TemplateEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DefaultDescription { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedByUserId { get; set; }
    }
}

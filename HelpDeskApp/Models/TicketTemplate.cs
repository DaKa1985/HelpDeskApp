using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class TicketTemplate : ICloneable
    {
        public string Title { get; set; }
        public string DefaultDescription { get; set; }
        public string Priority { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
        public object Clone()
        { 
            return this.MemberwiseClone(); 
        }
        public TicketTemplate DeepCopy()
        {
            var clone = (TicketTemplate)this.MemberwiseClone();
            //Das ist zunächst eine shallowcopy mit Titel, Priority und Description die 1zu1 kopiert werden...
            clone.Attachments = new List<string>(this.Attachments);
            //Für Attachments braucht es immer eine neue, eigene Liste...
            return clone;
        }
    }
}

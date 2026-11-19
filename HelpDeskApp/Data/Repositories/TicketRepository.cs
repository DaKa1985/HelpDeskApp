using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskApp.Data.Entities;

namespace HelpDeskApp.Data.Repositories
{
    public class TicketRepository
    {
        //Create
        public void Add(TicketEntity ticket)
        {
            using (var context = new HelpDeskContext())
            {
                context.Tickets.Add(ticket);
                context.SaveChanges();
            }
        }

        //Read - alle Tickets
        public List<TicketEntity> GetAll()
        {
            using (var context = new HelpDeskContext())
            { return context.Tickets
                    .Include(t => t.Attachments ) //lade alle Anhänge mit
                    .ToList();
            }
        }

        //Read - ein Ticket
        public TicketEntity GetById(int id)
        {
            using (var context = new HelpDeskContext())
            {
                return context.Tickets
                .Include(t => t.Attachments )
                .FirstOrDefault(t => t.Id == id);
            }
        }

        //Update
        public void Update(TicketEntity ticket)
        {
            using (var context = new HelpDeskContext())
            {
                context.Entry(ticket).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        //Delete
        public void Delete(int id)
        {
            using (var context = new HelpDeskContext())
            {
                var ticket = context.Tickets.Find(id);
                if (ticket != null)
                {
                    context.Tickets.Remove(ticket);
                    context.SaveChanges();
                }
            }
        }
    }
}

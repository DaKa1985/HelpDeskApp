using HelpDeskApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Data.Repositories
{
    public class LogRepository
    {
        //Create
        public void Add(LogEntity log)
        {
            using (var context = new HelpDeskContext())
            {
                context.LogEntries.Add(log);
                context.SaveChanges();
            }
        }

        //Read - alle Logs
        public List<LogEntity> GetAll()
        {
            using (var context = new HelpDeskContext())
            {
                return context.LogEntries
                    .ToList();
            }
        }

        //Read - ein Log
        public LogEntity Get(int id)
        {
            using (var context = new HelpDeskContext())
            {
                return context.LogEntries
                    .FirstOrDefault(l => l.Id == id);
            }
        }

        //Update
        public void Update(LogEntity log)
        {
            using (var context = new HelpDeskContext())
            {
                context.Entry(log).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        //Delete
        public void Delete(int id)
        {
            using (var context = new HelpDeskContext())
            {
                var log = context.LogEntries.Find(id);
                if (log != null)
                {
                    context.LogEntries.Remove(log);
                    context.SaveChanges();
                }
            }
        }

        public void ClearAll()
        {
            using (var context = new HelpDeskContext())
            {
                context.LogEntries.RemoveRange(context.LogEntries);
                context.SaveChanges();
            }
        }
    }
}

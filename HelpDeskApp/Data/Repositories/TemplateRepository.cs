using HelpDeskApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HelpDeskApp.Data.Repositories
{
    public class TemplateRepository
    {
        public void Add(TemplateEntity template)
        {
            using (var context = new HelpDeskContext())
            {
                context.Templates.Add(template);
                context.SaveChanges();
            }
        }

        public List<TemplateEntity> GetAll()
        {
            using (var context = new HelpDeskContext())
            {
                return context.Templates.ToList();
            }
        }

        public void Update(TemplateEntity template)
        {
            using (var context = new HelpDeskContext())
            {
                context.Entry(template).State =
                    System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var context = new HelpDeskContext())
            {
                var template = context.Templates.Find(id);
                if (template != null)
                {
                    context.Templates.Remove(template);
                    context.SaveChanges();
                }
            }
        }
    }
}
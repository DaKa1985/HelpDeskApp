using HelpDeskApp.Core.Services;
using HelpDeskApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Data.Repositories
{
    public class UserRepository
    {
        public void Add(UserEntity user)
        {
            using (var context = new HelpDeskContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public UserEntity GetByUsername(string username)
        {
            using (var context = new HelpDeskContext())
            {
                return context.Users
                    .FirstOrDefault(u => u.UserName == username);  // ← angepasst
            }
        }

        public bool UserExists(string username)
        {
            using (var context = new HelpDeskContext())
            {
                return context.Users
                    .Any(u => u.UserName == username);  // ← angepasst
            }
        }

        public void Update(UserEntity user)
        {
            using (var context = new HelpDeskContext())
            {
                context.Entry(user).State =
                    System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public List<UserEntity> GetAll()
        {
            using (var context = new HelpDeskContext())
            {
                return context.Users.ToList();
            }
        }
    }
}

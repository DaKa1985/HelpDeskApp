using HelpDeskApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Core.Services
{
    public static class SessionService
    {
        public static int CurrentUserId { get; private set; }
        public static string CurrentUsername { get; private set; }
        public static bool IsAdmin { get; private set; }
        public static bool IsLoggedIn => CurrentUsername != null;

        public static void Login(UserEntity user)
        {
            CurrentUserId = user.Id;
            CurrentUsername = user.UserName;
            IsAdmin = user.IsAdmin;
        }

        public static void Logout()
        {
            CurrentUserId = 0;
            CurrentUsername = null;
            IsAdmin = false;
        }
    }
}

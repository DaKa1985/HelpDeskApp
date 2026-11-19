using HelpDeskApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Core.Services
{
    public interface IDataService
    {
        //Tickets
        void SaveTicket(TicketEntity ticket);
        List<TicketEntity> GetAllTickets();
        void UpdateTicket(TicketEntity ticket);
        void DeleteTicket(int ticketId);

        //Log
        void SaveLog(LogEntity log);
        List<LogEntity> GetAllLogs();

        //User-Methoden
        void SaveUser(UserEntity user);
        UserEntity GetUser(string username);
        bool UserExists(string username);
        void UpdateUser(UserEntity user);
        List<UserEntity> GetAllUsers();

        // Template Methoden
        void SaveTemplate(TemplateEntity template);
        List<TemplateEntity> GetAllTemplates();
        void UpdateTemplate(TemplateEntity template);
        void DeleteTemplate(int id);
        void ClearLog();
    }
}

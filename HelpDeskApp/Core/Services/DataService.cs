using HelpDeskApp.Data.Entities;
using HelpDeskApp.Data.Repositories;
using System.Collections.Generic;


namespace HelpDeskApp.Core.Services
{
    public class DataService : IDataService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly LogRepository _logRepository;
        private readonly UserRepository _userRepository;
        private readonly TemplateRepository _templateRepository;

        public DataService()
        {
            _ticketRepository = new TicketRepository();
            _logRepository = new LogRepository();
            _userRepository = new UserRepository();
            _templateRepository = new TemplateRepository();
        }

        // Tickets:
        public void SaveTicket(TicketEntity ticket)
            => _ticketRepository.Add(ticket);

        public List<TicketEntity> GetAllTickets()
            => _ticketRepository.GetAll();

        public void UpdateTicket(TicketEntity ticket)
            => _ticketRepository.Update(ticket);

        public void DeleteTicket(int ticketId)
            => _ticketRepository.Delete(ticketId);

        // Log:
        public void SaveLog(LogEntity log)
            => _logRepository.Add(log);

        public List<LogEntity> GetAllLogs()
            => _logRepository.GetAll();

        // User-Methoden:
        public void SaveUser(UserEntity user)
            => _userRepository.Add(user);
        public UserEntity GetUser(string username)
            => _userRepository.GetByUsername(username);
        public bool UserExists(string username)
            => _userRepository.UserExists(username);
        public void UpdateUser(UserEntity user)
            => _userRepository.Update(user);
        public List<UserEntity> GetAllUsers()
            => _userRepository.GetAll();

        // Template-Methoden:
        public void SaveTemplate(TemplateEntity template)
    => _templateRepository.Add(template);

        public List<TemplateEntity> GetAllTemplates()
            => _templateRepository.GetAll();

        public void UpdateTemplate(TemplateEntity template)
            => _templateRepository.Update(template);

        public void DeleteTemplate(int id)
            => _templateRepository.Delete(id);

        public void ClearLog()
    => _logRepository.ClearAll();
        
    }
}
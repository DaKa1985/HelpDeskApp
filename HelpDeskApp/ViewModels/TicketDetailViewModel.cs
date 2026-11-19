using HelpDeskApp.Core;
using HelpDeskApp.Core.Services;
using HelpDeskApp.Data.Entities;
using HelpDeskApp.Helpers;
using HelpDeskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace HelpDeskApp.ViewModels
{
    public class TicketDetailViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly ITicket _ticket;

        public event Action SaveSuccessful;

        // Ticket Info (readonly):
        public string TicketTitle => _ticket.Title;
        public string TicketDescription => _ticket.Description;
        public string TicketLocation => _ticket.Location;

        // Status:
        public List<string> StatusOptionen { get; } = new List<string>
        {
            "Offen",
            "InBearbeitung",
            "Geloest",
            "Geschlossen"
        };

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }

        // Benutzer:
        public List<UserEntity> Benutzer { get; private set; }

        private UserEntity _selectedBenutzer;
        public UserEntity SelectedBenutzer
        {
            get => _selectedBenutzer;
            set
            {
                _selectedBenutzer = value;
                OnPropertyChanged();
            }
        }

        // Interne Notiz:
        private string _internalNote;
        public string InternalNote
        {
            get => _internalNote;
            set
            {
                _internalNote = value;
                OnPropertyChanged();
            }
        }

        private string _newNote;
        public string NewNote
        {
            get => _newNote;
            set
            {
                _newNote = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public TicketDetailViewModel(ITicket ticket)
        {
            _dataService = new DataService();
            _ticket = ticket;

            // Aktuellen Status laden:
            _selectedStatus = ticket.Status ?? "Offen";

            // Interne Notiz laden:
            _internalNote = ticket.InternalNote;

            // Alle Benutzer laden:
            Benutzer = _dataService.GetAllUsers();

            // Commands:
            SaveCommand = new RelayCommand(() => Save());
        }

        private void Save()
        {
            try
            {
                // Neue Notiz mit Zeitstempel anhängen:
                if (!string.IsNullOrEmpty(NewNote))
                {
                    var zeitstempel = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    var neueNotiz = $"{zeitstempel} | {SessionService.CurrentUsername}: {NewNote}";

                    // An bestehende Notizen anhängen:
                    if (string.IsNullOrEmpty(_ticket.InternalNote))
                        _ticket.InternalNote = neueNotiz;
                    else
                        _ticket.InternalNote += "\n" + neueNotiz;

                    InternalNote = _ticket.InternalNote;
                }

                // Status und Zuweisung:
                _ticket.Status = SelectedStatus;
                if (SelectedBenutzer != null)
                    _ticket.AssignedToUserId = SelectedBenutzer.Id;

                // In Datenbank updaten:
                var tickets = _dataService.GetAllTickets();
                var entity = tickets.FirstOrDefault(
                    t => t.Title == _ticket.Title &&
                         t.CreatedAt == _ticket.CreatedAt);

                if (entity != null)
                {
                    entity.Status = SelectedStatus;
                    entity.InternalNote = _ticket.InternalNote;
                    entity.UpdatedAt = DateTime.Now;
                    entity.AssignedToUserId = SelectedBenutzer?.Id;
                    _dataService.UpdateTicket(entity);
                }

                SaveSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Ticket speichern");
            }
        }
    }
}
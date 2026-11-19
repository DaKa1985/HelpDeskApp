using System;
using HelpDeskApp.Data.Entities;
using HelpDeskApp.Data.Repositories;
using HelpDeskApp.Helpers;
using HelpDeskApp.Models;
using HelpDeskApp.Patterns.AbstractFactory;
using HelpDeskApp.Patterns.Builder;
using HelpDeskApp.Patterns.Factory;
using HelpDeskApp.Patterns.Singleton;
using HelpDeskApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using HelpDeskApp.Core;
using HelpDeskApp.Core.Services;

namespace HelpDeskApp.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        // A1: Singleton 
        private AppLogger _logger = AppLogger.GetInstance();

        //Repositories für die Datenbank:
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        // A2: Abstract Factory
        private ITicketFactory _ticketFactory;

        private string _newTicketDescription;
        public string NewTicketDescription
        {
            get => _newTicketDescription;
            set
            {
                _newTicketDescription = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _newTicketTitle;
        public string NewTicketTitle
        {
            get => _newTicketTitle;
            set
            {
                _newTicketTitle = value;
                OnPropertyChanged();
            }
        }

        private string _newTicketLocation;

        public string NewTicketLocation
        {
            get => _newTicketLocation;
            set
            {
                _newTicketLocation = value;
                OnPropertyChanged();
            }
        }

        private bool _isExpanderOpen;

        public bool IsExpanderOpen
        {
            get => _isExpanderOpen;
            set
            {
                _isExpanderOpen = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> NewTicketAttachments { get; }
            = new ObservableCollection<string>();


        // A3: Factory Method – welcher Creator?
        private NotificationCreator _notifier;

        // Steuert den Benachrichtigungskanal
        private bool _notifyByEmail;
        public bool NotifyByEmail
        {
            get => _notifyByEmail;
            set
            {
                _notifyByEmail = value;
                OnPropertyChanged(nameof(NotifyByEmail));
                OnPropertyChanged(nameof(NotifyByConsole));

                // Welchen Creator setzt du hier je nach Wert?
                if (_notifyByEmail)
                    _notifier = new EmailCreator();
                else
                    _notifier = new ConsoleCreator();
            }
        }

        public bool NotifyByConsole
        {
            get => !_notifyByEmail;
            set
            {
                _notifyByEmail = !value;
                OnPropertyChanged(nameof(NotifyByEmail));
                OnPropertyChanged(nameof(NotifyByConsole));
                if (_notifyByEmail)
                    _notifier = new EmailCreator();
                else
                    _notifier = new ConsoleCreator();
            }
        }

        // A4: Prototype/ Vorlagen
        //private readonly List<TicketTemplate> _templates = new List<TicketTemplate>();
        private readonly ObservableCollection<TicketTemplate> _templates = new ObservableCollection<TicketTemplate>();
        public IReadOnlyList<TicketTemplate> Templates => _templates;

        private List<TicketTemplate> _geklonteVorlagen = new List<TicketTemplate>();
        private TicketTemplate _selectedTemplate;
        public TicketTemplate SelectedTemplate 
        {
            get=> _selectedTemplate;
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged();
            }
        }


        // A5: Builder und Director
        private IReportBuilder _reportBuilder;
        private ReportDirector _director = new ReportDirector();

        private string _dailyReport;
        public string DailyReport
        {
            get => _dailyReport;
            set
            {
                _dailyReport = value;
                OnPropertyChanged();
            }
        }

        //Collections
        public ObservableCollection<ITicket> Tickets { get; } =
            new ObservableCollection<ITicket>();

        private ITicket _selectedTicket;
        public ITicket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); // ← Buttons aktivieren!
            }
        }

        public bool IsAdmin => SessionService.IsAdmin;
        public string CurrentUser => SessionService.CurrentUsername;

        //Log
        public string LogText
        {
            get
            {
                //Alle Log-einträge mit Zeilenumbruch zusammenfügen
                return string.Join("\n", _logger.GetAllEntries());
            }
        }

        //Commands
        public ICommand BenutzerverwaltungCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand AttachmentHinzufuegenCommand { get; }
        public ICommand AttachmentEntfernenCommand { get; }
        public ICommand KritischesTicketCommand { get; }
        public ICommand NormalesTicketCommand { get; }
        public ICommand VorlageKlonenCommand { get; }
        public ICommand TicketBearbeitenCommand { get; }
        public ICommand TicketLoeschenCommand { get; }
        public ICommand AlsVorlageCommand { get; }
        public ICommand VorlageAnlegenCommand { get; }
        public ICommand TagesberichtCommand { get; }
        public ICommand LogExportierenCommand { get; }
        public ICommand LogLeerenCommand { get; }


        //Konstruktor
        public DashboardViewModel()
        {
            //Repositories/ Services initialisieren
            _dataService = new DataService();
            _navigationService = new NavigationService();

            //Tickets aus Datenbank laden:
            LoadTicketsFromDatabase();

            //Vorlagen aus Datenbank laden:
            LoadTemplatesFromDatabase();

            //Standardmäßig Consolenbenachrichtigung festlegen
            _notifier = new ConsoleCreator();
            _notifyByEmail = false;

            BenutzerverwaltungCommand = new RelayCommand(
                execute: () => new UserManagementWindow().ShowDialog(),
                canExecute: () => IsAdmin
            );

            LogoutCommand = new RelayCommand(() => Logout());

            AttachmentHinzufuegenCommand = new RelayCommand(
                execute: () =>
                {
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.Title = "Attachment auswählen";
                    dialog.Filter = "Alle Dateien (*.*)|*.*";

                    if (dialog.ShowDialog() == true)
                    {
                        NewTicketAttachments.Add(dialog.FileName);
                    }
                }
            );

            AttachmentEntfernenCommand = new RelayCommand<string>(
                execute: (attachment) => NewTicketAttachments.Remove(attachment)
            );

            KritischesTicketCommand = new RelayCommand(
                execute: () => CreateCriticalTicket(NewTicketDescription),
                canExecute: () =>!string.IsNullOrEmpty(NewTicketDescription)
            );

            NormalesTicketCommand = new RelayCommand(
                execute: () => CreateNormalTicket(NewTicketDescription),
                canExecute: () => !string.IsNullOrEmpty(NewTicketDescription)
            );

            TicketBearbeitenCommand = new RelayCommand(
                execute: () => TicketBearbeiten(),
                canExecute: () => SelectedTicket != null && IsAdmin
            );

            TicketLoeschenCommand = new RelayCommand(
                execute: () => TicketLoeschen(),
                canExecute: () => SelectedTicket != null && IsAdmin
            );

            AlsVorlageCommand = new RelayCommand(
                execute: () => AlsVorlageSpeiichern(),
                canExecute: () => SelectedTicket != null && IsAdmin
            );

            VorlageKlonenCommand = new RelayCommand(
                execute: () => CloneSelectedTemplate(),
                canExecute: () => SelectedTemplate != null
            );

            VorlageAnlegenCommand = new RelayCommand(
                () => VorlageAnlegen(),
                () => IsAdmin
            );

            TagesberichtCommand = new RelayCommand(
                () => GenerateDailyReport()
            );

            LoadLogFromDatabase();

            LogExportierenCommand = new RelayCommand(
                execute: () => LogExportieren(),
                canExecute: () => IsAdmin
);

            LogLeerenCommand = new RelayCommand(
                execute: () => LogLeeren(),
                canExecute: () => IsAdmin
            );
        }

        //Öffentliche Methoden

        //Button: "Abmelden"
        private void Logout()
        {
            if (_navigationService.Confirm("Möchtest du dich wirklich abmelden?"))
            {
                SessionService.Logout();

                // Neues LoginWindow öffnen:
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Dashboard schließen:
                System.Windows.Application.Current.MainWindow.Close();
            }
        }

        // Tickets aus der Datenbank laden und in ObservableCollection umwandeln:
        private void LoadTicketsFromDatabase()
        {
            // Alle User einmal laden:
            var users = _dataService.GetAllUsers();

            var tickets = _dataService.GetAllTickets();
            foreach (var entity in tickets)
            {
                var ticket = entity.Priority == "KRITISCH"
                    ? (ITicket)new CriticalTicket()
                    : new NormalTicket();

                ticket.Title = entity.Title;
                ticket.Description = entity.Description;
                ticket.Location = entity.Location;
                ticket.CreatedAt = entity.CreatedAt;
                ticket.Status = entity.Status ?? "Offen";
                ticket.InternalNote = entity.InternalNote;
                ticket.AssignedToUserId = entity.AssignedToUserId;

                // Username über ID nachschlagen:
                var creator = users.FirstOrDefault(
                    u => u.Id == entity.CreatedByUserId);
                ticket.CreatedByUsername = creator?.UserName ?? "Unbekannt";

                ticket.Attachments = entity.Attachments
                    .Select(a => a.FilePath)
                    .ToList();

                Tickets.Add(ticket);
            }
        }
        //Button: "Kritisches Ticket erstellen"
        public void CreateCriticalTicket(string description)
        {
            // A2: Fabrikmethode wählen:
            _ticketFactory = new CriticalTicketFactory();

            // Fabrik benutzen:
            var ticket = _ticketFactory.CreateTicket(description);
            ticket.Title = NewTicketTitle;
            ticket.Description = NewTicketDescription;
            ticket.Location = NewTicketLocation;
            ticket.CreatedAt = DateTime.Now;
            ticket.CreatedByUsername = SessionService.CurrentUsername;
            ticket.Attachments = new List<string>(NewTicketAttachments);

            var escalation = _ticketFactory.CreateEscalation();

            // Ticket zur Liste hinzufügen:
            Tickets.Add(ticket);

            //Iticket zu TicketEntity umwandeln...
            var entity = new TicketEntity
            {
                Priority = ticket.Priority,
                Title = ticket.Title,
                Description = ticket.Description,
                Location = ticket.Location,
                CreatedAt = ticket.CreatedAt,
                CreatedByUserId = SessionService.CurrentUserId,  // ← HIER!
                Attachments = ticket.Attachments
            .Select(a => new AttachmentEntity { FilePath = a })
            .ToList()
            };

            //... und in Datenbank speichern
            _dataService.SaveTicket(entity) ;


            // A3: Benachrichtigung auslösen:
            _notifier.Notify("Neues Ticket" + ticket.Description);

            // A1: Loggen:
            Log("Kritisches Ticket erstellt: " + ticket.Description);

            //Leeren der Felder
            NewTicketTitle = string.Empty;
            NewTicketDescription = string.Empty;
            NewTicketLocation = string.Empty;
            NewTicketAttachments.Clear();
        }

        //Button: "Normales Ticket erstellen"
        public void CreateNormalTicket(string description)
        {
            // A2: Fabrikmethode wählen:
            _ticketFactory = new NormalTicketFactory();

            // Fabrik benutzen:
            var ticket = _ticketFactory.CreateTicket(description);
            ticket.Title= NewTicketTitle;
            ticket.Description = NewTicketDescription;
            ticket.Location = NewTicketLocation;
            ticket.CreatedAt = DateTime.Now;
            ticket.CreatedByUsername = SessionService.CurrentUsername;
            ticket.Attachments = new List<string>(NewTicketAttachments);

            var escalation = _ticketFactory.CreateEscalation();

            // Ticket zur Liste hinzufügen:
            Tickets.Add(ticket);
            //Iticket zu TicketEntity umwandeln...
            var entity = new TicketEntity
            {
                Priority = ticket.Priority,
                Title = ticket.Title,
                Description = ticket.Description,
                Location = ticket.Location,
                CreatedAt = ticket.CreatedAt,
                CreatedByUserId = SessionService.CurrentUserId,  // ← HIER!
                Attachments = ticket.Attachments
            .Select(a => new AttachmentEntity { FilePath = a })
            .ToList()
            };

            //... und in Datenbank speichern
            _dataService.SaveTicket(entity);

            // A3: Benachrichtigung auslösen:
            _notifier.Notify("Neues Ticket" + ticket.Description);

            // A1: Loggen:
            Log("Normales Ticket erstellt: " + ticket.Description);

            //Leeren der Felder
            NewTicketTitle= string.Empty;
            NewTicketDescription = string.Empty;
            NewTicketLocation = string.Empty;
            NewTicketAttachments.Clear();
        }

        private void TicketBearbeiten()
        {
            // Ticket zwischenspeichern!
            var ticket = SelectedTicket;  // ← NEU!

            var window = new TicketDetailWindow(ticket);
            if (window.ShowDialog() == true)
            {
                // Liste aktualisieren:
                var index = Tickets.IndexOf(ticket);
                Tickets.Remove(ticket);
                Tickets.Insert(index, ticket);

                Log("Ticket bearbeitet: " + ticket.Title);  // ← ticket statt SelectedTicket!
            }
        }

        private void TicketLoeschen()
        {
            // Ticket zwischenspeichern!
            var ticket = SelectedTicket;  // ← NEU!

            if (!_navigationService.Confirm(
                $"Ticket '{ticket.Title}' wirklich löschen?"))
                return;

            var tickets = _dataService.GetAllTickets();
            var entity = tickets.FirstOrDefault(
                t => t.Title == ticket.Title &&
                     t.CreatedAt == ticket.CreatedAt);

            if (entity != null)
                _dataService.DeleteTicket(entity.Id);

            Tickets.Remove(ticket);
            Log("Ticket gelöscht: " + ticket.Title);  // ← ticket statt SelectedTicket!
        }
        private void AlsVorlageSpeiichern()
        {
            var template = new TemplateEntity
            {
                Title = SelectedTicket.Title,
                DefaultDescription = SelectedTicket.Description,
                Priority = SelectedTicket.Priority,
                CreatedByUserId = SessionService.CurrentUserId
            };

            _dataService.SaveTemplate(template);
            LoadTemplatesFromDatabase();
            Log("Vorlage aus Ticket erstellt: " + SelectedTicket.Title);
        }

        //Button "Vorlage Klonen"
        public void CloneSelectedTemplate()
        {
            var klon = SelectedTemplate.DeepCopy();
            _geklonteVorlagen.Add(klon);

            //Felder mit Vorlagen-Daten befüllen:
            NewTicketTitle = klon.Title;
            NewTicketDescription = klon.DefaultDescription;
            NewTicketLocation = "";

            //Expander aufklappen:
            IsExpanderOpen = true;


            Log("Vorlage geklont: " + SelectedTemplate.Title);
        }

        public void VorlageAnlegen() 
        {
            var window = new TemplateWindow();
            if (window.ShowDialog() == true)
            {
                // Vorlagen neu laden:
                LoadTemplatesFromDatabase();
            }
        }

        private void LoadTemplatesFromDatabase()
        {
            _templates.Clear();

            var templates = _dataService.GetAllTemplates();
            foreach (var entity in templates)
            {
                _templates.Add(new TicketTemplate
                {
                    Title = entity.Title,
                    DefaultDescription = entity.DefaultDescription,
                    Priority = entity.Priority
                });
            }
        }

        // Button: "Tagesbericht"
        public void GenerateDailyReport()
        {
            // A5: Builder vorbereiten
            _reportBuilder = new TextReportBuilder();

            // Director steuert die Reihenfolge – nicht wir!
            _director.BuildDailyReport (_reportBuilder, Tickets.ToList());

            // Fertigen Bericht holen:
            var report = _reportBuilder.GetResult();

            // A1: Loggen:
            Log("Tagesbericht exportiert");

            // Tickets nummerieren:
            var numerierteListe = report.TicketLines
                .Select((line, index) => $"{index + 1}. {line}")
                .ToList();

            // Property setzen, UI wird automatisch aktualisiert
            DailyReport =   report.Header + "\n" +
                            "__________________________________________\n\n" +
                            string.Join("\n", numerierteListe) + "\n" +
                            "__________________________________________\n" +
                            report.Statistics;
            //Popup öffnen:
            _navigationService.ShowReport(DailyReport);
        }

        private void LoadLogFromDatabase()
        {
            var logs = _dataService.GetAllLogs();
            foreach (var log in logs)
            {
                _logger.LoadEntry(log.Message);
            }
            OnPropertyChanged(nameof(LogText));
        }

        private void LogExportieren()
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Title = "Log exportieren";
                dialog.Filter = "Textdateien (*.txt)|*.txt";
                dialog.FileName = "HelpDesk_Log_" +
                                  DateTime.Now.ToString("dd_MM_yyyy");

                if (dialog.ShowDialog() != true) return;

                // Log-Einträge in Datei schreiben:
                var eintraege = _logger.GetAllEntries();
                System.IO.File.WriteAllLines(dialog.FileName, eintraege);

                Log("Log exportiert nach: " + dialog.FileName);
                System.Windows.MessageBox.Show(
                    "Log erfolgreich exportiert!", "Export");
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Log exportieren");
            }
        }

        private void LogLeeren()
        {
            if (!_navigationService.Confirm(
                "Log wirklich leeren? Diese Aktion kann nicht rückgängig gemacht werden!"))
                return;

            // Aus DB leeren:
            _dataService.ClearLog();

            // Singleton leeren:
            _logger.ClearLog();

            OnPropertyChanged(nameof(LogText));
            Log("Log wurde geleert von: " + SessionService.CurrentUsername);
        }

        //private Hilfsmethoden
        private void Log(string message)    
        {   
            _logger.Log(message);
            OnPropertyChanged(nameof(LogText));

            _dataService.SaveLog(new LogEntity
            {
                Message = message
            });
        }
    }
}


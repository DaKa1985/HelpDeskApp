using BCrypt.Net;
using HelpDeskApp.Core;
using HelpDeskApp.Core.Services;
using HelpDeskApp.Data.Entities;
using System;
using System.Windows.Input;
using HelpDeskApp.Helpers;

namespace HelpDeskApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        // Event für erfolgreichen Login:
        public event Action LoginSuccessful;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // Passwort kommt aus Code-Behind – kein Binding!
        public string Password { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            _dataService = new DataService();

            // Admin anlegen falls noch nicht vorhanden:
            CreateAdminIfNotExists();
        }

        public bool LoginWasSuccessful { get; private set; }
        public void Login()
        {
            try
            {
                // Felder prüfen:
                if (string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Bitte Username und Passwort eingeben!";
                    return;
                }

                // User in DB suchen:
                var user = _dataService.GetUser(Username);
                if (user == null)
                {
                    ErrorMessage = "Benutzer nicht gefunden!";
                    return;
                }

                // Passwort prüfen:
                if (!BCrypt.Net.BCrypt.Verify(Password, user.HashedPassword))
                {
                    ErrorMessage = "Falsches Passwort!";
                    return;
                }

                if (!user.IsActive)
                {
                    ErrorMessage = "Dieser Account wurde deaktiviert!";
                    return;
                }
                //Session starten:
                SessionService.Login(user);
                LoginWasSuccessful = true;

                // Login erfolgreich!
                ErrorMessage = string.Empty;
                LoginSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Login");
            }
        }

        public void Register()
        {
            try
            {
                // Felder prüfen:
                if (string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Bitte Username und Passwort eingeben!";
                    return;
                }

                // Mindestlänge Passwort:
                if (Password.Length < 6)
                {
                    ErrorMessage = "Passwort muss mindestens 6 Zeichen haben!";
                    Password = string.Empty;
                    return;
                }

                // User bereits vorhanden?
                if (_dataService.UserExists(Username))
                {
                    ErrorMessage = "Benutzername bereits vergeben!";
                    Password = string.Empty;
                    return;
                }

                // Neuen User anlegen:
                var user = new UserEntity
                {
                    UserName = Username,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(Password),
                    IsAdmin = false  // ← Neue User sind nie Admin!
                };

                _dataService.SaveUser(user);

                Password = string.Empty; // Passwort aus Sicherheitsgründen löschen
                Username = string.Empty; // Username zurücksetzen

                ErrorMessage = "Registrierung erfolgreich! Bitte anmelden.";
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Registrierung");
            }
        }

        private void CreateAdminIfNotExists()
        {
            // Admin nur einmal anlegen:
            if (!_dataService.UserExists("admin"))
            {
                var admin = new UserEntity
                {
                    UserName = "admin",
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsAdmin = true
                };
                _dataService.SaveUser(admin);
            }
        }
    }
}
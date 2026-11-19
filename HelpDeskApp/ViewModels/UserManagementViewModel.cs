using HelpDeskApp.Core;
using HelpDeskApp.Core.Services;
using HelpDeskApp.Data.Entities;
using HelpDeskApp.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HelpDeskApp.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        public ObservableCollection<UserEntity> Benutzer { get; }
            = new ObservableCollection<UserEntity>();

        private UserEntity _selectedBenutzer;
        public UserEntity SelectedBenutzer
        {
            get => _selectedBenutzer;
            set
            {
                _selectedBenutzer = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AdminMachenCommand { get; }
        public ICommand AdminEntfernenCommand { get; }
        public ICommand AktivierenCommand { get; }
        public ICommand DeaktivierenCommand { get; }

        public UserManagementViewModel()
        {
            _dataService = new DataService();

            // Commands:
            AdminMachenCommand = new RelayCommand(
                execute: () => AdminMachen(),
                canExecute: () => SelectedBenutzer != null
                                  && !SelectedBenutzer.IsAdmin
            );
            
            AdminEntfernenCommand = new RelayCommand(
                execute: () => AdminEntfernen(),
                canExecute: () => SelectedBenutzer != null
                                  && SelectedBenutzer.IsAdmin
                                  && !IstLetzterAdmin()  // ← letzter Admin geschützt!
            );

            AktivierenCommand = new RelayCommand(
                execute: () => Aktivieren(),
                canExecute: () => SelectedBenutzer != null
                                  && !SelectedBenutzer.IsActive
            );

            DeaktivierenCommand = new RelayCommand(
                execute: () => Deaktivieren(),
                canExecute: () => SelectedBenutzer != null
                                  && SelectedBenutzer.IsActive
                                  && !IstLetzterAdmin()
            );

            // Benutzer laden:
            LoadBenutzer();
        }

        private void LoadBenutzer()
        {
            Benutzer.Clear();
            var users = _dataService.GetAllUsers();
            foreach (var user in users)
                Benutzer.Add(user);
        }

        private void AdminMachen()
        {
            var user = SelectedBenutzer;
            user.IsAdmin = true;
            _dataService.UpdateUser(user);
            LoadBenutzer();
        }

        private void AdminEntfernen()
        {
            var user = SelectedBenutzer;
            user.IsAdmin = false;
            _dataService.UpdateUser(user);
            LoadBenutzer();
        }

        private void Aktivieren()
        {
            var user = SelectedBenutzer;
            user.IsActive = true;
            _dataService.UpdateUser(user);
            LoadBenutzer();
        }

        private void Deaktivieren()
        {
            var user = SelectedBenutzer;
            user.IsActive = false;
            _dataService.UpdateUser(user);
            LoadBenutzer();
        }

        private bool IstLetzterAdmin()
        {
            // Verhindert dass letzter Admin deaktiviert wird!
            var admins = _dataService.GetAllUsers()
                .Where(u => u.IsAdmin && u.IsActive)
                .ToList();
            return admins.Count == 1
                   && admins[0].Id == SelectedBenutzer.Id;
        }
    }
}
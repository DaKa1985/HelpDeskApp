using BCrypt.Net;
using HelpDeskApp.Core;
using HelpDeskApp.Core.Services;
using HelpDeskApp.Data.Entities;
using HelpDeskApp.Helpers;
using System;
using System.Windows.Input;

namespace HelpDeskApp.ViewModels
{
    public class TemplateViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        // Event für erfolgreiches Speichern:
        public event Action SaveSuccessful;

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _defaultDescription;
        public string DefaultDescription
        {
            get => _defaultDescription;
            set
            {
                _defaultDescription = value;
                OnPropertyChanged();
            }
        }

        private bool _isKritisch;
        public bool IsKritisch
        {
            get => _isKritisch;
            set
            {
                _isKritisch = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNormal));
            }
        }

        public bool IsNormal
        {
            get => !_isKritisch;
            set
            {
                _isKritisch = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsKritisch));
            }
        }

        public ICommand SaveCommand { get; }

        public TemplateViewModel()
        {
            _dataService = new DataService();

            // Standard: Normal
            _isKritisch = false;

            SaveCommand = new RelayCommand(
                execute: () => Save(),
                canExecute: () => !string.IsNullOrEmpty(Title)
            );
        }

        private void Save()
        {
            try
            {
                var template = new TemplateEntity
                {
                    Title = Title,
                    DefaultDescription = DefaultDescription,
                    Priority = _isKritisch ? "KRITISCH" : "NORMAL",
                    CreatedByUserId = SessionService.CurrentUserId
                };

                _dataService.SaveTemplate(template);
                SaveSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex, "Vorlage speichern");
            }
        }
    }
}
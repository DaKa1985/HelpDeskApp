using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HelpDeskApp.Helpers
{
    public class RelayCommand : ICommand
    {
        // 1. Hier wird die Methode gespeichert die ausgeführt werden soll
        //    Action = eine Methode die nichts zurückgibt (void)
        private readonly Action _execute;
        // 2. Hier wird gespeichert WANN der Button aktiv sein soll
        //    Func<bool> = eine Methode die true oder false zurückgibt
        //    = null bedeutet: optional – nicht immer nötig!
        private readonly Func<bool> _canExecute;

        // 3. Konstruktor – hier übergibst du deine Methode
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // 4. WPF fragt: "Darf der Button aktiv sein?"
        public bool CanExecute(object parameter)
        {
            // Wenn kein canExecute angegeben → immer aktiv!
            // Wenn canExecute angegeben → frag es ab!
            return _canExecute == null || _canExecute();
        }

        // 5. WPF sagt: "Button wurde geklickt – mach was!"
        public void Execute(object parameter)
        {
            _execute();
        }

        // 6. WPF fragt regelmäßig: "Hat sich CanExecute geändert?"
        //    CommandManager.RequerySuggested kümmert sich automatisch darum!
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

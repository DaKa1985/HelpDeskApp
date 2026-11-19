using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskApp.Patterns.Singleton
{
    public class AppLogger
    {
        // 1. Die einzige Instanz wird hier aufbewahrt
        private static AppLogger _instance;
        private List<string> _logEntries = new List<string>();

        // 2. Niemand von außen darf new AppLogger() aufrufen
        private AppLogger() { }

        // 3. Alle rufen DIESE Methode auf
        public static AppLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppLogger();
            }

            return _instance;
        }

        public void Log(string message)
        {
            var logEntry = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " | " + message;
            _logEntries.Add(logEntry);
        }

        public IReadOnlyCollection<string> GetAllEntries()
        {
            return _logEntries.AsReadOnly();
        }

        public void LoadEntry(string message)
        {
            _logEntries.Add(message);
        }

        public void ClearLog()
        {
            _logEntries.Clear();
        }
    }
}

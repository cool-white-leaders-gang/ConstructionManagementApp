using System;
using ConstructionManagementApp.App.Interfaces;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.Services
{
    internal class LogService : ILogService
    {
        private readonly LogRepository _logRepository; 

        public LogService(LogRepository logRepository)
        {
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }

        public void OnActionOccurred(object sender, LogEventArgs e)
        {
            try
            {
                var log = new Log(e.Message, e.Timestamp, e.UserName);

                _logRepository.AddLog(log);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class LogController
    {
        private readonly LogRepository _logRepository;

        public LogController(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void AddLog(string message)
        {
            try
            {
                Log log = new Log(message, DateTime.Now);
                _logRepository.AddLog(log);
                Console.WriteLine("Log został pomyślnie dodany.");
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

        public void DisplayAllLogs()
        {
            try
            {
                var logs = _logRepository.GetAllLogs();
                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów w systemie.");
                    return;
                }

                Console.WriteLine("--- Lista logów ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void DisplayLogsByMessage(string message)
        {
            try
            {
                var logs = _logRepository.GetLogsByMessage(message);
                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów pasujących do podanej wiadomości.");
                    return;
                }

                Console.WriteLine("--- Logi pasujące do wiadomości ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        public void DisplayLogsByDate(DateTime date)
        {
            try
            {
                var logs = _logRepository.GetLogsByDate(date);
                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów z podanej daty.");
                    return;
                }

                Console.WriteLine("--- Logi z podanej daty ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class LogController
    {
        private readonly LogRepository _logRepository;

        // Konstruktor kontrolera logów
        public LogController(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        // Dodaje nowy log do repozytorium.
        public void AddLog(string message, string userName)
        {
            try
            {
                // Tworzy nowy log z aktualną datą.
                Log log = new Log(message, DateTime.Now, userName);

                // Dodaje log do repozytorium.
                _logRepository.AddLog(log);
                Console.WriteLine("Log został pomyślnie dodany.");
            }
            catch (ArgumentException ex)
            {
                // Obsługa błędu w przypadku niepoprawnych danych.
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Obsługa nieoczekiwanych błędów.
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        // Wyświetla wszystkie logi zapisane w systemie.
        public void DisplayAllLogs()
        {
            try
            {
                // Pobiera wszystkie logi z repozytorium.
                var logs = _logRepository.GetAllLogs();

                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów w systemie.");
                    return;
                }

                // Wyświetla listę logów.
                Console.WriteLine("--- Lista logów ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}][{log.UserName}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów podczas wyświetlania.
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Wyświetla logi, które zawierają określoną wiadomość.
        public void DisplayLogsByMessage(string message)
        {
            try
            {
                // Pobiera logi zawierające dany tekst.
                var logs = _logRepository.GetLogsByMessage(message);

                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów pasujących do podanej wiadomości.");
                    return;
                }

                // Wyświetla znalezione logi.
                Console.WriteLine("--- Logi pasujące do wiadomości ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów.
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        // Wyświetla logi z konkretnej daty.
        public void DisplayLogsByDate(DateTime date)
        {
            try
            {
                // Pobiera logi z wybranej daty.
                var logs = _logRepository.GetLogsByDate(date);

                if (logs.Count == 0)
                {
                    Console.WriteLine("Brak logów z podanej daty.");
                    return;
                }

                // Wyświetla logi z danej daty.
                Console.WriteLine("--- Logi z podanej daty ---");
                foreach (var log in logs)
                {
                    Console.WriteLine($"[{log.Timestamp}] {log.Message}");
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędów.
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}

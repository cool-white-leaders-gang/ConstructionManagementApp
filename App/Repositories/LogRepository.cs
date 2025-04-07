using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class LogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log nie może być null.");

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public List<Log> GetAllLogs()
        {
            return _context.Logs.OrderByDescending(l => l.Timestamp).ToList();
        }

        public List<Log> GetLogsByMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Wiadomość logu nie może być pusta.");

            return _context.Logs
                .Where(l => l.Message.Contains(message, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }

        public List<Log> GetLogsByDate(DateTime date)
        {
            return _context.Logs
                .Where(l => l.Timestamp.Date == date.Date)
                .OrderByDescending(l => l.Timestamp)
                .ToList();
        }
    }
}
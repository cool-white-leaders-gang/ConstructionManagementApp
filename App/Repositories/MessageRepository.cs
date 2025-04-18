using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class MessageRepository
    {
        private readonly AppDbContext _context;

        // Konstruktor repozytorium, inicjalizuje kontekst bazy danych
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        // Wysłanie wiadomości do odbiorcy
        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Wiadomość nie może być null.");

            // Sprawdzenie, czy nadawca istnieje w systemie
            if (!_context.Users.Any(u => u.Id == message.SenderId))
                throw new ArgumentException("Nadawca nie istnieje w systemie.");

            // Sprawdzenie, czy odbiorca istnieje w systemie
            if (!_context.Users.Any(u => u.Id == message.ReceiverId))
                throw new ArgumentException("Odbiorca nie istnieje w systemie.");

            // Dodanie wiadomości do bazy danych i zapisanie zmian
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        // Pobranie wiadomości dla użytkownika na podstawie jego Id
        public List<Message> GetMessagesForUser(int userId)
        {
            // Sprawdzenie, czy użytkownik istnieje w systemie
            if (!_context.Users.Any(u => u.Id == userId))
                throw new ArgumentException("Użytkownik nie istnieje w systemie.");

            // Pobranie wiadomości, które zostały wysłane do podanego użytkownika
            return _context.Messages
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)  // Sortowanie po dacie wysłania (od najnowszych)
                .ToList();
        }

        // Pobranie Id użytkownika na podstawie jego nazwy użytkownika (username)
        public int GetUserIdByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new ArgumentException($"Użytkownik o nazwie '{username}' nie istnieje.");
            return user.Id;
        }
    }
}

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

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Wiadomość nie może być null.");

            if (!_context.Users.Any(u => u.Id == message.SenderId))
                throw new ArgumentException("Nadawca nie istnieje w systemie.");

            if (!_context.Users.Any(u => u.Id == message.ReceiverId))
                throw new ArgumentException("Odbiorca nie istnieje w systemie.");

            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public List<Message> GetMessagesForUser(int userId)
        {
            if (!_context.Users.Any(u => u.Id == userId))
                throw new ArgumentException("Użytkownik nie istnieje w systemie.");

            return _context.Messages
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToList();
        }

        public int GetUserIdByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new ArgumentException($"Użytkownik o nazwie '{username}' nie istnieje.");
            return user.Id;
        }
    }
}
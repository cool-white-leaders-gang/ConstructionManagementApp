using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MessageController
    {
        private readonly MessageRepository _messageRepository;
        private readonly UserRepository _userRepository;

        public MessageController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public void SendMessage(Message message)
        {
            try
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message), "Wiadomość nie może być null.");

                _messageRepository.SendMessage(message);
                Console.WriteLine("Wiadomość została pomyślnie wysłana.");
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

        public void SendMessageByUsername(int senderId, string receiverUsername, string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(receiverUsername))
                    throw new ArgumentException("Nazwa użytkownika odbiorcy nie może być pusta.");

                if (string.IsNullOrWhiteSpace(content))
                    throw new ArgumentException("Treść wiadomości nie może być pusta.");

                var receiverId = _messageRepository.GetUserIdByUsername(receiverUsername);

                Message message = new Message(senderId, receiverId, content);

                _messageRepository.SendMessage(message);
                Console.WriteLine("Wiadomość została pomyślnie wysłana.");
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

        public void DisplayMessagesForUser(int userId)
        {
            try
            {
                var messages = _messageRepository.GetMessagesForUser(userId);

                if (messages.Count == 0)
                {
                    Console.WriteLine("Brak wiadomości.");
                    return;
                }

                foreach (var message in messages)
                {
                    Console.WriteLine($"Od: {_userRepository.GetUserById(message.SenderId)}, Treść: {message.Content}, Data: {message.SentAt}");
                }
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
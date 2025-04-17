using System;
using System.Collections.Generic;
using ConstructionManagementApp.App.Repositories;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.Events;

namespace ConstructionManagementApp.App.Controllers
{
    internal class MessageController
    {
        private readonly MessageRepository _messageRepository;
        private readonly UserRepository _userRepository;
        private readonly AuthenticationService _authenticationService;
        public event LogEventHandler MessageSent;



        public MessageController(UserRepository userRepository, MessageRepository messageRepository, AuthenticationService authenticationService)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _authenticationService = authenticationService;
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
                MessageSent?.Invoke(this, new LogEventArgs(_authenticationService.CurrentSession.User.Username, $"Wysłano wiadomość do {receiverUsername} o treści: \"{content}\""));
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
                    var sender = _userRepository.GetUserById(message.SenderId);
                    if (sender == null)
                    {
                        Console.WriteLine("Nie znaleziono użytkownika nadawcy.");
                        continue;
                    }
                    Console.WriteLine($"Od: {sender.Email}, Treść: {message.Content}, Data: {message.SentAt}");
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
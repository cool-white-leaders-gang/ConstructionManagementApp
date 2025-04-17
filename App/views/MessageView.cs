using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class MessageView
    {
        private readonly MessageController _messageController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public MessageView(MessageController messageController, RBACService rbacService, User currentUser)
        {
            _messageController = messageController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView(User currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine($"--- Wiadomości ---");
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Wyświetl wiadomości");
                Console.WriteLine("2. Wyślij wiadomość po nazwie użytkownika odbiorcy");
                Console.WriteLine("3. Powrót do menu głównego");

                Console.Write("\nTwój wybór: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            if (HasPermission(Permission.SendMessage)) DisplayMessages(currentUser.Id);
                            break;
                        case 2:
                            if (HasPermission(Permission.ViewMessages)) SendMessageByUsername(currentUser.Id);
                            break;
                        case 3:
                            isRunning = false; // Powrót do menu głównego
                            break;
                        default:
                            Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }

                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować.");
            }
        }

        private bool HasPermission(Permission permission)
        {
            if (!_rbacService.HasPermission(_currentUser, permission))
            {
                Console.WriteLine("Brak uprawnień do wykonania tej operacji.");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        private void DisplayMessages(int userId)
        {
            Console.Clear();
            Console.WriteLine("--- Twoje wiadomości ---");
            _messageController.DisplayMessagesForUser(userId);
        }

        private void SendMessageByUsername(int senderId)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Wyślij wiadomość ---");

                Console.Write("Podaj nazwę użytkownika odbiorcy: ");
                var receiverUsername = Console.ReadLine();

                Console.Write("Podaj treść wiadomości: ");
                var content = Console.ReadLine();

                _messageController.SendMessageByUsername(senderId, receiverUsername, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }
    }
}
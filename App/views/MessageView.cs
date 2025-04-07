using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Views
{
    internal class MessageView
    {
        private readonly MessageController _messageController;

        public MessageView(MessageController messageController)
        {
            _messageController = messageController;
        }

        public void ShowView(User currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine($"Zalogowano jako: {currentUser.Username}");
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
                            DisplayMessages(currentUser.Id);
                            break;
                        case 2:
                            SendMessageByUsername(currentUser.Id);
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
                Console.ReadKey();
            }
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
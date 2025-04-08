using System;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Views;
using ConstructionManagementApp.App.Services;

namespace ConstructionManagementApp.App.Views
{
    internal class MainMenu
    {
        private readonly Session _session;
        private readonly UserView _userView;

        public MainMenu(Session session, UserView userView)
        {
            _session = session;
            _userView = userView;
        }

        public void Show()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine($"Witaj!");
                Console.WriteLine("Wybierz opcję:");
                Console.WriteLine("1. Zarządzanie użytkownikami");
                Console.WriteLine("2. Zarządzanie projektami");
                Console.WriteLine("3. Zarządzanie zespołami");
                Console.WriteLine("4. Zarządzanie zadaniami");
                Console.WriteLine("5. Zarządzanie materiałami");
                Console.WriteLine("6. Zarządzanie sprzętem");
                Console.WriteLine("7. Zarządzanie budżetami");
                Console.WriteLine("8. Zarządzanie raportami postępu");
                Console.WriteLine("9. Zarządzanie zgłoszeniami problemów");
                Console.WriteLine("0. Wyloguj się");

                Console.Write("\nTwój wybór: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        _userView.ShowView();
                        break;
                    case 2:
                        Console.WriteLine("Funkcjonalność zarządzania projektami nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine("Funkcjonalność zarządzania zespołami nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Funkcjonalność zarządzania zadaniami nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.WriteLine("Funkcjonalność zarządzania materiałami nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.WriteLine("Funkcjonalność zarządzania sprzętem nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 7:
                        Console.WriteLine("Funkcjonalność zarządzania budżetami nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 8:
                        Console.WriteLine("Funkcjonalność zarządzania raportami postępu nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 9:
                        Console.WriteLine("Funkcjonalność zarządzania zgłoszeniami problemów nie jest jeszcze zaimplementowana.");
                        Console.ReadKey();
                        break;
                    case 0:
                        Console.WriteLine("Wylogowywanie...");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
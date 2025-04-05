using System;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Utilities;

namespace ConstructionManagementApp.App.Views
{
    internal class UserView
    {
        private readonly RBACService _rbacService;
        private readonly UserController _userController;

        public UserView(RBACService rbacService, UserController userController)
        {
            _rbacService = rbacService;
            _userController = userController;
        }

        public void ShowView(User user)
        {
            bool loggedIn = true;

            while (loggedIn)
            {
                Console.Clear();
                Console.WriteLine($"Zalogowano jako: {user.Username}");
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Wyświetl listę użytkowników");

                if (_rbacService.HasPermission(user, Permission.CreateUser))
                    Console.WriteLine("2. Dodaj nowego użytkownika");

                if (_rbacService.HasPermission(user, Permission.UpdateUser))
                    Console.WriteLine("3. Zaktualizuj dane użytkownika");

                if (_rbacService.HasPermission(user, Permission.DeleteUser))
                    Console.WriteLine("4. Usuń użytkownika");

                Console.WriteLine("5. Wyloguj się");
                Console.WriteLine("6. Wyjdź z programu");

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
                        DisplayAllUsers();
                        break;
                    case 2:
                        if (_rbacService.HasPermission(user, Permission.CreateUser))
                            AddUser();
                        else
                            ShowNoPermissionMessage();
                        break;
                    case 3:
                        if (_rbacService.HasPermission(user, Permission.UpdateUser))
                            UpdateUser();
                        else
                            ShowNoPermissionMessage();
                        break;
                    case 4:
                        if (_rbacService.HasPermission(user, Permission.DeleteUser))
                            DeleteUser();
                        else
                            ShowNoPermissionMessage();
                        break;
                    case 5:
                        Console.WriteLine("Wylogowano.");
                        loggedIn = false;
                        break;
                    case 6:
                        Console.WriteLine("Zamykanie programu...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayAllUsers()
        {
            Console.Clear();
            Console.WriteLine("--- Lista użytkowników ---");
            _userController.DisplayAllUsers();
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private void AddUser()
        {
            Console.Clear();
            Console.WriteLine("--- Dodaj nowego użytkownika ---");
            Console.Write("Podaj nazwę użytkownika: ");
            var username = Console.ReadLine();

            Console.Write("Podaj adres e-mail: ");
            var email = Console.ReadLine();

            Console.Write("Podaj hasło: ");
            var password = Console.ReadLine();

            Console.WriteLine("Wybierz rolę: 1. Admin, 2. Manager, 3. Worker, 4. Client");
            var roleChoice = Console.ReadLine();
            Role role = roleChoice switch
            {
                "1" => Role.Admin,
                "2" => Role.Manager,
                "3" => Role.Worker,
                "4" => Role.Client,
                _ => throw new ArgumentException("Wybrano nieprawidłową rolę.")
            };

            try
            {
                var hashedPassword = PasswordHasher.HashPassword(password);
                var user = new User(username, email, hashedPassword, role);
                _userController.AddUser(user);
                Console.WriteLine("Użytkownik został pomyślnie dodany.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("--- Zaktualizuj dane użytkownika ---");
            
            Console.Write("Podaj nową nazwę użytkownika: ");
            var username = Console.ReadLine();

            Console.Write("Podaj nowy adres e-mail: ");
            var email = Console.ReadLine();

            Console.Write("Podaj nowe hasło: ");
            var password = Console.ReadLine();

            Console.WriteLine("Wybierz nową rolę: 1. Admin, 2. Manager, 3. Worker, 4. Client");
            var roleChoice = Console.ReadLine();
            Role role = roleChoice switch
            {
                "1" => Role.Admin,
                "2" => Role.Manager,
                "3" => Role.Worker,
                "4" => Role.Client,
                _ => throw new ArgumentException("Wybrano nieprawidłową rolę.")
            };

            try
            {
                var hashedPassword = PasswordHasher.HashPassword(password);
                var user = new User(username, email, hashedPassword, role);
                _userController.UpdateUser(user);
                Console.WriteLine("Dane użytkownika zostały pomyślnie zaktualizowane.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("--- Usuń użytkownika ---");
            Console.Write("Podaj ID użytkownika: ");
            if (!int.TryParse(Console.ReadLine(), out var userId))
            {
                Console.WriteLine("Nieprawidłowe ID.");
                Console.ReadKey();
                return;
            }

            try
            {
                _userController.DeleteUser(userId);
                Console.WriteLine("Użytkownik został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private void ShowNoPermissionMessage()
        {
            Console.WriteLine("Nie masz uprawnień do wykonania tej operacji.");
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }
    }
}
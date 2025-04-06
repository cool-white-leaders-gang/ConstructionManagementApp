using System;
using System.Collections.Generic;
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

        public void ShowView(User currentUser)
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine($"Zalogowano jako: {currentUser.Username} ({currentUser.Role})");
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Wyświetl listę użytkowników");
                Console.WriteLine("2. Wyszukaj użytkownika");
                if (_rbacService.HasPermission(currentUser, Permission.CreateUser))
                    Console.WriteLine("3. Dodaj nowego użytkownika");
                if (_rbacService.HasPermission(currentUser, Permission.UpdateUser))
                    Console.WriteLine("4. Zaktualizuj dane użytkownika");
                if (_rbacService.HasPermission(currentUser, Permission.DeleteUser))
                    Console.WriteLine("5. Usuń użytkownika");
                Console.WriteLine("6. Wyloguj się");

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
                            DisplayAllUsers();
                            break;
                        case 2:
                            SearchUser();
                            break;
                        case 3:
                            if (_rbacService.HasPermission(currentUser, Permission.CreateUser))
                                AddUser();
                            else
                                ShowNoPermissionMessage();
                            break;
                        case 4:
                            if (_rbacService.HasPermission(currentUser, Permission.UpdateUser))
                                UpdateUser();
                            else
                                ShowNoPermissionMessage();
                            break;
                        case 5:
                            if (_rbacService.HasPermission(currentUser, Permission.DeleteUser))
                                DeleteUser();
                            else
                                ShowNoPermissionMessage();
                            break;
                        case 6:
                            Console.WriteLine("Wylogowano.");
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
                }

                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
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

        private void SearchUser()
        {
            Console.Clear();
            Console.WriteLine("--- Wyszukaj użytkownika ---");
            Console.WriteLine("1. Wyszukaj po ID");
            Console.WriteLine("2. Wyszukaj po nazwie użytkownika");
            Console.WriteLine("3. Wyszukaj po adresie e-mail");
            Console.Write("\nTwój wybór: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby wrócić do menu.");
                Console.ReadKey();
                return;
            }

            try
            {
                switch (choice)
                {
                    case 1:
                        Console.Write("Podaj ID użytkownika: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            var user = _userController.GetUserById(id);
                            DisplayUser(user);
                        }
                        else
                        {
                            Console.WriteLine("Niepoprawne ID.");
                        }
                        break;
                    case 2:
                        Console.Write("Podaj nazwę użytkownika: ");
                        var username = Console.ReadLine();
                        var usersByUsername = _userController.GetUserByUsername(username);
                        DisplayUsers(usersByUsername);
                        break;
                    case 3:
                        Console.Write("Podaj adres e-mail: ");
                        var email = Console.ReadLine();
                        var userByEmail = _userController.GetUserByEmail(email);
                        DisplayUser(userByEmail);
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

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
            catch(ArgumentException ex){
                Console.WriteLine($"Błąd: {ex.Message}");
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
            Console.Write("Podaj ID użytkownika: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Niepoprawne ID.");
                Console.ReadKey();
                return;
            }

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
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Niepoprawne ID.");
                Console.ReadKey();
                return;
            }

            try
            {
                _userController.DeleteUser(id);
                Console.WriteLine("Użytkownik został pomyślnie usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }

        private void DisplayUser(User user)
        {
            if (user == null)
            {
                Console.WriteLine("Nie znaleziono użytkownika.");
            }
            else
            {
                Console.WriteLine("\n--- Szczegóły użytkownika ---");
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Nazwa użytkownika: {user.Username}");
                Console.WriteLine($"E-mail: {user.Email}");
                Console.WriteLine($"Rola: {user.Role}");
            }
        }

        private void DisplayUsers(List<User> users)
        {
            if (users == null || users.Count == 0)
            {
                Console.WriteLine("Nie znaleziono użytkowników.");
            }
            else
            {
                Console.WriteLine("\n--- Lista użytkowników ---");
                foreach (var user in users)
                {
                    Console.WriteLine($"ID: {user.Id}, Nazwa: {user.Username}, E-mail: {user.Email}, Rola: {user.Role}");
                }
            }
        }

        private void ShowNoPermissionMessage()
        {
            Console.WriteLine("Nie masz uprawnień do wykonania tej operacji.");
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu.");
            Console.ReadKey();
        }
    }
}
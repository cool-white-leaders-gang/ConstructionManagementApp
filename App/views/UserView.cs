using System;
using ConstructionManagementApp.App.Controllers;
using ConstructionManagementApp.App.Enums;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Services;
using ConstructionManagementApp.App.Utilities;

namespace ConstructionManagementApp.App.Views
{
    internal class UserView
    {
        private readonly UserController _userController;
        private readonly RBACService _rbacService;
        private readonly User _currentUser;

        public UserView(UserController userController, RBACService rbacService, User currentUser)
        {
            _userController = userController;
            _rbacService = rbacService;
            _currentUser = currentUser;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie użytkownikami ---");
                Console.WriteLine("1. Wyświetl wszystkich użytkowników");
                Console.WriteLine("2. Dodaj nowego użytkownika");
                Console.WriteLine("3. Zaktualizuj użytkownika");
                Console.WriteLine("4. Usuń użytkownika");
                Console.WriteLine("5. Wyszukaj użytkownika");
                Console.WriteLine("6. Powrót do menu głównego");

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
                        if (HasPermission(Permission.ViewUsers)) DisplayAllUsers();
                        break;
                    case 2:
                        if (HasPermission(Permission.CreateUser)) AddUser();
                        break;
                    case 3:
                        if (HasPermission(Permission.UpdateUser)) UpdateUser();
                        break;
                    case 4:
                        if (HasPermission(Permission.DeleteUser)) DeleteUser();
                        break;
                    case 5:
                        if (HasPermission(Permission.ViewUsers)) SearchUser();
                        break;
                    case 6:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
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

        private void DisplayAllUsers()
        {
            Console.Clear();
            _userController.DisplayAllUsers();
            ReturnToMenu();
        }

        private void AddUser()
        {
            try
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

                var hashedPassword = PasswordHasher.HashPassword(password);
                _userController.AddUser(username, email, hashedPassword, role);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void UpdateUser()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj dane użytkownika ---");

                Console.Write("Podaj nazwę użytkownika do zaktualizowania: ");
                var userToUpdate = Console.ReadLine();

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
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Hasło nie może być puste.");
                }
                var hashedPassword = PasswordHasher.HashPassword(password);
                _userController.UpdateUser(userToUpdate, username, email, hashedPassword, role);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void DeleteUser()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń użytkownika ---");

                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();

                _userController.DeleteUser(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            finally
            {
                ReturnToMenu();
            }
        }

        private void SearchUser()
        {
            Console.Clear();
            Console.WriteLine("--- Wyszukaj użytkownika ---");
            Console.WriteLine("1. Wyszukaj po nazwie użytkownika");
            Console.WriteLine("2. Wyszukaj po adresie e-mail");
            Console.WriteLine("3. Powrót do menu użytkowników");

            Console.Write("\nTwój wybór: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                Console.ReadKey();
                return;
            }

            switch (choice)
            {
                case 1:
                    SearchByUsername();
                    break;
                case 2:
                    SearchByEmail();
                    break;
                case 3:
                    return; // Powrót do menu użytkowników
                default:
                    Console.WriteLine("Niepoprawny wybór.");
                    break;
            }

            ReturnToMenu();
        }

        private void SearchByUsername()
        {
            try
            {
                Console.Write("Podaj nazwę użytkownika: ");
                var username = Console.ReadLine();

                var user = _userController.GetUserByUsername(username);
                if (user != null)
                {
                    Console.WriteLine($"Znaleziono użytkownika: ID: {user.Id}, Nazwa: {user.Username}, E-mail: {user.Email}, Rola: {user.Role}");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono użytkownika o podanej nazwie.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        private void SearchByEmail()
        {
            try
            {
                Console.Write("Podaj adres e-mail: ");
                var email = Console.ReadLine();

                var user = _userController.GetUserByEmail(email);
                if (user != null)
                {
                    Console.WriteLine($"Znaleziono użytkownika: ID: {user.Id}, Nazwa: {user.Username}, E-mail: {user.Email}, Rola: {user.Role}");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono użytkownika o podanym adresie e-mail.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
        }

        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu użytkowników.");
            Console.ReadKey();
        }
    }
}
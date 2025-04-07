using System;
using ConstructionManagementApp.App.Controllers;

namespace ConstructionManagementApp.App.Views
{
    internal class ProgressReportView
    {
        private readonly ProgressReportController _progressReportController;

        public ProgressReportView(ProgressReportController progressReportController)
        {
            _progressReportController = progressReportController;
        }

        public void ShowView()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("--- Zarządzanie raportami postępu ---");
                Console.WriteLine("1. Wyświetl wszystkie raporty postępu");
                Console.WriteLine("2. Dodaj nowy raport postępu");
                Console.WriteLine("3. Zaktualizuj raport postępu");
                Console.WriteLine("4. Usuń raport postępu");
                Console.WriteLine("5. Powrót do menu głównego");

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
                        DisplayAllProgressReports();
                        break;
                    case 2:
                        AddProgressReport();
                        break;
                    case 3:
                        UpdateProgressReport();
                        break;
                    case 4:
                        DeleteProgressReport();
                        break;
                    case 5:
                        isRunning = false; // Powrót do menu głównego
                        break;
                    default:
                        Console.WriteLine("Niepoprawny wybór. Naciśnij dowolny klawisz, aby spróbować ponownie.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void DisplayAllProgressReports()
        {
            Console.Clear();
            _progressReportController.DisplayAllProgressReports();
            ReturnToMenu();
        }

        private void AddProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Dodaj nowy raport postępu ---");

                Console.Write("Podaj tytuł raportu: ");
                var title = Console.ReadLine();

                Console.Write("Podaj opis raportu: ");
                var description = Console.ReadLine();

                Console.Write("Podaj datę raportu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var date))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                _progressReportController.AddProgressReport(title, description, date);
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

        private void UpdateProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Zaktualizuj raport postępu ---");

                Console.Write("Podaj ID raportu: ");
                if (!int.TryParse(Console.ReadLine(), out var reportId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                Console.Write("Podaj nowy tytuł raportu: ");
                var title = Console.ReadLine();

                Console.Write("Podaj nowy opis raportu: ");
                var description = Console.ReadLine();

                Console.Write("Podaj nową datę raportu (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out var date))
                {
                    Console.WriteLine("Niepoprawny format daty.");
                    return;
                }

                _progressReportController.UpdateProgressReport(reportId, title, description, date);
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

        private void DeleteProgressReport()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Usuń raport postępu ---");

                Console.Write("Podaj ID raportu: ");
                if (!int.TryParse(Console.ReadLine(), out var reportId))
                {
                    Console.WriteLine("Niepoprawne ID.");
                    return;
                }

                _progressReportController.DeleteProgressReport(reportId);
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

        private void ReturnToMenu()
        {
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby wrócić do menu raportów postępu.");
            Console.ReadKey();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class ProgressReportRepository
    {
        private readonly AppDbContext _context;
        public ProgressReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateProgressReport(ProgressReport progressReport)
        {
            _context.ProgressReports.Add(progressReport);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy raport");
        }
        public void UpdateProgressReport(ProgressReport progressReport)
        {
            try
            {
                _context.ProgressReports.Update(progressReport);
                _context.SaveChanges();
                Console.WriteLine("Raport zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego raportu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        

        public void DeleteProgressReport(int reportId)
        {
            var report = _context.ProgressReports.FirstOrDefault(e => reportId == e.Id);
            if (report == null)
            {
                Console.WriteLine("Nie ma takiego raportu");
                return;
            }
            _context.ProgressReports.Remove(report);
            report = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Raport usunięty");
        }

        public List<ProgressReport> GetAllReports()
        {
            return _context.ProgressReports.ToList();
        }

        public ProgressReport GetReportById(int reportId)
        {
            return _context.ProgressReports.FirstOrDefault(e => reportId == e.Id);
        }
    }
}

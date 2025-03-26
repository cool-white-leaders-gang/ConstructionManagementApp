using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class ReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateReport(Report report)
        {
            _context.Reports.Add(report);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy raport");
        }
        public void UpdateReport(Report report)
        {
            try
            {
                _context.Reports.Update(report);
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

        public void DeleteReport(int reportId)
        {
            var report = _context.Reports.FirstOrDefault(e => reportId == e.Id);
            if (report == null)
            {
                Console.WriteLine("Nie ma takiego raportu");
                return;
            }
            _context.Reports.Remove(report);
            report = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Raport usunięty");
        }

        public List<Report> GetAllReports()
        {
            return _context.Reports.ToList();
        }

        public Report GetReportById(int reportId)
        {
            return _context.Reports.FirstOrDefault(e => reportId == e.Id);
        }
    }
}

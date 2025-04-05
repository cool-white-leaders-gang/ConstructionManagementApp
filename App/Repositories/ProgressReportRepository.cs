using System.Collections.Generic;
using System.Linq;
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

        public void CreateProgressReport(ProgressReport report)
        {
            _context.ProgressReports.Add(report);
            _context.SaveChanges();
        }

        public void UpdateProgressReport(ProgressReport report)
        {
            var existingReport = GetProgressReportById(report.Id);
            if (existingReport == null)
                throw new KeyNotFoundException("Nie znaleziono raportu postępu o podanym Id.");

            _context.ProgressReports.Update(report);
            _context.SaveChanges();
        }

        public void DeleteProgressReport(int reportId)
        {
            var report = GetProgressReportById(reportId);
            if (report == null)
                throw new KeyNotFoundException("Nie znaleziono raportu postępu o podanym Id.");

            _context.ProgressReports.Remove(report);
            _context.SaveChanges();
        }

        public ProgressReport GetProgressReportById(int reportId)
        {
            return _context.ProgressReports.FirstOrDefault(r => r.Id == reportId);
        }

        public List<ProgressReport> GetAllProgressReports()
        {
            return _context.ProgressReports.ToList();
        }
    }
}

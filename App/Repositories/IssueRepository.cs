using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class IssueRepository
    {
        private readonly AppDbContext _context;
        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateIssue(Issue issue)
        {
            _context.Issues.Add(issue);
            _context.SaveChanges();
            Console.WriteLine("Utworzono nowy problem");
        }
        public void UpdateIssue(Issue issue)
        {
            try
            {
                _context.Issues.Update(issue);
                _context.SaveChanges();
                Console.WriteLine("Problem zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego problemu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DeleteIssue(int issueId)
        {
            var issue = _context.Issues.FirstOrDefault(e => issueId == e.Id);
            if (issue == null)
            {
                Console.WriteLine("Nie ma takiego problemu");
                return;
            }
            _context.Issues.Remove(issue);
            issue = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Problem usunięty");
        }

        public List<Issue> GetAllIssues()
        {
            return _context.Issues.ToList();
        }

        public Issue GetIssueById(int issueId)
        {
            return _context.Issues.FirstOrDefault(e => issueId == e.Id);
        }


    }
}

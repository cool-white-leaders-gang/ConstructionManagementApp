using System;
using System.Collections.Generic;
using System.Linq;
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
            if (issue == null)
                throw new ArgumentNullException(nameof(issue), "Zgłoszenie nie może być null.");

            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void UpdateIssue(Issue issue)
        {
            var existingIssue = GetIssueById(issue.Id);
            if (existingIssue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia o podanym Id.");

            existingIssue.Title = issue.Title;
            existingIssue.Description = issue.Description;
            existingIssue.Status = issue.Status;

            _context.Issues.Update(existingIssue);
            _context.SaveChanges();
        }

        public void DeleteIssueById(int issueId)
        {
            var issue = GetIssueById(issueId);
            if (issue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia o podanym Id.");

            _context.Issues.Remove(issue);
            _context.SaveChanges();
        }

        public Issue GetIssueById(int id)
        {
            return _context.Issues.FirstOrDefault(i => i.Id == id);
        }

        public List<Issue> GetAllIssues()
        {
            return _context.Issues.ToList();
        }
    }
}

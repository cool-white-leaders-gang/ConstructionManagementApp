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
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public void UpdateIssue(Issue issue)
        {
            var existingIssue = GetIssueById(issue.Id);
            if (existingIssue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia problemu o podanym Id.");

            _context.Issues.Update(issue);
            _context.SaveChanges();
        }

        public void DeleteIssue(int issueId)
        {
            var issue = GetIssueById(issueId);
            if (issue == null)
                throw new KeyNotFoundException("Nie znaleziono zgłoszenia problemu o podanym Id.");

            _context.Issues.Remove(issue);
            _context.SaveChanges();
        }

        public Issue GetIssueById(int issueId)
        {
            return _context.Issues.FirstOrDefault(i => i.Id == issueId);
        }

        public List<Issue> GetAllIssues()
        {
            return _context.Issues.ToList();
        }
    }
}

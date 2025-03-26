using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionManagementApp.App.Database;
using ConstructionManagementApp.App.Models;

namespace ConstructionManagementApp.App.Repositories
{
    internal class TeamRepository
    {
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
            Console.WriteLine("Dodano nowy zespół");
        }
        public void UpdateTeam(Team team)
        {
            try
            {
                _context.Teams.Update(team);
                _context.SaveChanges();
                Console.WriteLine("Zespół zaktualizowany");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Próba aktualizacji nieistniejącego zespołu: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void DeleteTeam(int teamId)
        {
            var team = _context.Teams.FirstOrDefault(e => teamId == e.Id);
            if (team == null)
            {
                Console.WriteLine("Nie ma takiego zespołu");
                return;
            }
            _context.Teams.Remove(team);
            team = null;
            GC.Collect();
            _context.SaveChanges();
            Console.WriteLine("Zespół usunięty");
        }

        public List<Team> GetAllTeams()
        {
            return _context.Teams.ToList();
        }

        public Team GetTeamById(int teamId)
        {
            return _context.Teams.FirstOrDefault(e => teamId == e.Id);
        }
    }
}

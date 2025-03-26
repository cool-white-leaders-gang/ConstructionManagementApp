
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    //klasa user
    internal class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        // Lista przypisanych zadań (przez TaskAssignment)
        public List<TaskAssignment> TaskAssignments { get; set; }
        public List<TeamMembers> TeamMembers { get; set; }      //dodanie nowej tabeli laczacej

        //konstruktor
        public User(string username, string passwordHash, string email, Role role)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            TaskAssignments = new List<TaskAssignment>();
            TeamMembers = new List<TeamMembers>();
        }

        public override string ToString()
        {
            return $"Nazwa użytkownika: {Username}, email: {Email}, rola: {Role}";
        }

    }
}

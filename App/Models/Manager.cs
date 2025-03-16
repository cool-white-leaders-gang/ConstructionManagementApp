
namespace ConstructionManagementApp.App.Models
{
    internal class Manager : User
    {
        public string Department {  get; set; }
        public Manager(string username, string passwordHash, string email, string department)
            : base(username, passwordHash, email, Enums.Role.Manager)
        {
            Department = department;
        }
        public override string ToString()
        {
            return base.ToString() + $", Dział: {Department}";
        }
    }
}

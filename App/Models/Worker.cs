namespace ConstructionManagementApp.App.Models
{
    internal class Worker : User
    {
        public string Position { get; set; }
        public Worker(string username, string passwordHash, string email, string position)
            : base(username, passwordHash, email, Enums.Role.Admin)
        {
            Position = position;
        }
        public override string ToString()
        {
            return base.ToString() + $", Stanowisko: {Position}";
        }
    }
}

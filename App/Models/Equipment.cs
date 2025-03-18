using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Equipment(string name, string status)
        {
            Name = name;
            Status = status;
        }

        public override string ToString()
        {
            return $"Equipment: {Name}, Status: {Status}";
        }
    }
}
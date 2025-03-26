using System;
using ConstructionManagementApp.App.Enums;
namespace ConstructionManagementApp.App.Models
{
    internal class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentStatus Status { get; set; }
        public int ProjectId { get; set; }

        public Equipment(string name, EquipmentStatus status, int projectId)
        {
            Name = name;
            Status = status;
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return $"Equipment: {Name}, Status: {Status}";
        }
    }
}
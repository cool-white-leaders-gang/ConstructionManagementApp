using System;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Models
{
    internal class Equipment
    {
        public int Id { get; private set; }

        private string _name;
        private EquipmentStatus _status;
        private int _projectId;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa sprzętu nie może być pusta.");
                _name = value;
            }
        }

        public EquipmentStatus Status
        {
            get => _status;
            set
            {
                if (!Enum.IsDefined(typeof(EquipmentStatus), value))
                    throw new ArgumentException("Nieprawidłowy status sprzętu.");
                _status = value;
            }
        }

        public int ProjectId
        {
            get => _projectId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id projektu musi być większe od zera.");
                _projectId = value;
            }
        }

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
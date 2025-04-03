using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Material
    {
        public int Id { get; private set; }

        private string _name;
        private int _quantity;
        private string _unit;
        private int _projectId;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa materiału nie może być pusta.");
                _name = value;
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Ilość materiału nie może być ujemna.");
                _quantity = value;
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Jednostka materiału nie może być pusta.");
                _unit = value;
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

        public Material(string name, int quantity, string unit, int projectId)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return $"Materiał: {Name}, Ilość: {Quantity} {Unit}";
        }
    }
}
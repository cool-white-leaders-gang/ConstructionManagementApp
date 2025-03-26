using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Material
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public int ProjectId { get; set; }

        public Material(string name, int quantity, string unit, int projectId)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            ProjectId = projectId;
        }

        public override string ToString()
        {
            return $"Mateira³: {Name}, Iloœæ: {Quantity} {Unit}";
        }
    }
}
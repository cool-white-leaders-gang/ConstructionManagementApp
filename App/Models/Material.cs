using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public Material(string name, int quantity, string unit)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
        }

        public override string ToString()
        {
            return $"Material: {Name}, Quantity: {Quantity} {Unit}";
        }
    }
}
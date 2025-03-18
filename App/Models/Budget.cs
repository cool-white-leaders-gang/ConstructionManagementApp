using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Budget
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SpentAmount { get; set; }

        public Budget(decimal totalAmount)
        {
            TotalAmount = totalAmount;
            SpentAmount = 0;
        }

        public void Spend(decimal amount)
        {
            SpentAmount += amount;
        }

        public override string ToString()
        {
            return $"Budget: Total Amount: {TotalAmount}, Spent Amount: {SpentAmount}";
        }
    }
}
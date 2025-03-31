using System;

namespace ConstructionManagementApp.App.Models
{
    internal class Budget
    {
        public int Id { get; private set; }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Budżet nie może być mniejszy niż 0");
                if (value < SpentAmount)
                    throw new InvalidOperationException("Budżet nie może być mniejszy niż wydana wartość");
                _totalAmount = value;
            }
        }

        private decimal _spentAmount;
        public decimal SpentAmount
        {
            get => _spentAmount;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Wydana wartość nie możę być mniejsza od 0");
                if (value > TotalAmount)
                    throw new InvalidOperationException("Wydana wartośc nie może być wyższa niż budżet");
                _spentAmount = value;
            }
        }

        public Budget(decimal totalAmount)
        {
            TotalAmount = totalAmount;
            SpentAmount = 0;
        }
    
        public Budget() { }
    }
}
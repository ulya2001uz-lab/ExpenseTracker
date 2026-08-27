using System;

namespace ExpenseTracker 
{
    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Core
{    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}

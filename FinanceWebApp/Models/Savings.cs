using System;
using System.Collections.Generic;

namespace FinanceWebApp.Models
{
    public partial class Savings
    {
        public int SavingsId { get; set; }
        public int UsersId { get; set; }
        public decimal SavingAmount { get; set; }
        public int SavingYears { get; set; }
        public string SavingReason { get; set; }
        public int SavingInterestrate { get; set; }
        public string MonthlyAmountTosave { get; set; }

        public virtual Users Users { get; set; }
    }
}

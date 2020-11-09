using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceLibrary
{
    public class Saving
    {
        public Saving(double amount, double years, string reason, double interestRate)
        {
            Amount = amount;
            Years = years;
            Reason = reason;
            SavingInterestRate = interestRate;
        }

        public double Amount { get; set; }
        public double Years  { get; set; }
        public string Reason { get; set; }
        public double SavingInterestRate { get; set; }


        public double MonthsToSave()
        {
            double amount = Amount * (1 + SavingInterestRate / 100);
            double yearsToMonths = Years * 12;
            double total = amount / yearsToMonths;
            return total;
        }
    }
}

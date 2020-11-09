using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceLibrary
{
    public class RentalExpense : General
    {
        public RentalExpense(double monthlyRental)
        {
            MonthlyRental = monthlyRental;
        }

        public RentalExpense()
        {

        }

        //Rent fields
        public static double MonthlyRental { get; set; }

        //Returns the monthly rental cost
        public double MonthlyRentalExpenses()
        {
            return MonthlyRental;
        }
    }
}

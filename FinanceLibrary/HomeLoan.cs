using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FinanceLibrary
{
    public class HomeLoan : GeneralExpense
    {
        public HomeLoan()
        {

        }
        
        //Home constructor that recieves those fields
        public HomeLoan(double homePurchasePrice, double homeTotalDeposit, double homeInterestRate, double homeMonthsRepay)
        {
            HomePurchasePrice = homePurchasePrice;
            HomeTotalDeposit = homeTotalDeposit;
            HomeInterestRate = homeInterestRate;
            HomeMonthsRepay = homeMonthsRepay;
        }

        //Home fields
        public static double HomePurchasePrice { get; set; }
        public static double HomeTotalDeposit { get; set; }
        public static double HomeInterestRate { get; set; }
        public static double HomeMonthsRepay { get; set; }


        //Total home repayment with simple interest 
        public double TotalHomeLoanRepayment()
        {
            if (HomePurchasePrice != 0 && HomeTotalDeposit != 0 && HomeInterestRate != 0 && HomeMonthsRepay != 0)
            {
                var totalRepayment = (HomePurchasePrice - HomeTotalDeposit) * (1 + (HomeInterestRate / 100 * (HomeMonthsRepay / 12)));
                return totalRepayment;
            }
            else
            {
                return 0;
            }
        }

        //Monthly home repayment with simple interest
        public double MonthlyHomeLoanRepayment()
        {
            if (HomePurchasePrice != 0 && HomeTotalDeposit != 0 && HomeInterestRate != 0 && HomeMonthsRepay != 0)
            {
                var monthlyRepayment = (HomePurchasePrice - HomeTotalDeposit) * (1 + (HomeInterestRate / 100 * (HomeMonthsRepay / 12))) / HomeMonthsRepay;
                return monthlyRepayment;
            }
            else
            {
                return 0;
            }
        }

    }
}

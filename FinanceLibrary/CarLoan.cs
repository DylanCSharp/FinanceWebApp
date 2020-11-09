using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceLibrary
{
    public class CarLoan : GeneralExpense
    {
        public CarLoan()
        {

        }

        public CarLoan(string carModelMake, double carPurchasePrice, double carDeposit, double carInterestRate, double carInsurancePremium)
        {
            CarModelMake = carModelMake;
            CarPurchasePrice = carPurchasePrice;
            CarDeposit = carDeposit;
            CarInterestRate = carInterestRate;
            CarInsurancePremium = carInsurancePremium;
        }

        //Car Loan fields
        public static string CarModelMake { get; set; }
        public static double CarPurchasePrice { get; set; }
        public static double CarDeposit { get; set; }
        public static double CarInterestRate { get; set; }
        public static double CarInsurancePremium { get; set; }

        //Total cost of car calculated with simple interest
        public double CarLoanRepaymentTotal()
        {
            
            var total = (CarPurchasePrice - CarDeposit) * (1 + (CarInterestRate / 100 * 5));
            return total;
        }

        //Monthly cost of car with simple interest
        public double CarLoanRepaymentMonthly()
        {
            var submonthly = (CarPurchasePrice - CarDeposit) * (1 + (CarInterestRate / 100 * 5)) / 60;
            var monthly = submonthly + CarInsurancePremium;
            return monthly;
        }
    }
}

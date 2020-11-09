using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FinanceLibrary
{
    public abstract class GeneralExpense
    {
        public GeneralExpense()
        {
            
        }

        public GeneralExpense(double grossIncome, double taxDeducted, double groceries, double waterLights, double travelCosts, double phoneCosts, double otherExpenses)
        {
            GrossIncome = grossIncome;
            TaxDeducted = taxDeducted;
            Groceries = groceries;
            WaterLights = waterLights;
            TravelCosts = travelCosts;
            PhoneCosts = phoneCosts;
            OtherExpenses = otherExpenses;
        }
        //Normal Expenses Fields
        public static double GrossIncome { get; set; }
        public static double TaxDeducted { get; set; }
        public static double Groceries { get; set; }
        public static double WaterLights { get; set; }
        public static double TravelCosts { get; set; }
        public static double PhoneCosts { get; set; }
        public static double OtherExpenses { get; set; }

        
        //Adding all the normal expenses together
        public static double NormalExpenses()
        {
            var expenses = Groceries + WaterLights + TravelCosts + PhoneCosts + OtherExpenses;
            return expenses;
        }

        //Income after Tax
        public static double FinalIncome()
        {
            var income = GrossIncome - TaxDeducted;
            return income;
        }

        //Money after deductions have been made depending on what the user chooses
        public static double AvailableMoneyAfterDeductions()
        {
            HomeLoan home = new HomeLoan();
            CarLoan car = new CarLoan();
            if (home.MonthlyHomeLoanRepayment() != 0)
            {
                double income = FinalIncome();
                double homeInstallments = home.MonthlyHomeLoanRepayment();

                double carInstallements = car.CarLoanRepaymentMonthly();

                double incomeLeftOver = income - homeInstallments - carInstallements - NormalExpenses();

                return incomeLeftOver;
            }
            else
            {
                RentalExpense rental = new RentalExpense();
                double income = FinalIncome();
                double rentalInstallment = rental.MonthlyRentalExpenses();

                double incomeleftOver = income - rentalInstallment - NormalExpenses() - car.CarLoanRepaymentMonthly();

                return incomeleftOver;
            }
        }

        //Spendable income after final income after tax, minus normal expenses
        public static double SpendableIncome()
        {
            double spendable = FinalIncome() - NormalExpenses();
            return spendable;
        }
    }
}

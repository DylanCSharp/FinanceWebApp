using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceLibrary;
using Microsoft.Data.SqlClient;
using FinanceWebApp.Models;
using Microsoft.Extensions.Configuration;

namespace FinanceWebApp.Controllers
{
    public class DataController : Controller
    {
        readonly FinanceApplicationContext _context;
        readonly IConfiguration _configuration;

        public DataController(FinanceApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult General()
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    return View();
                }
                else
                {
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult General(double income, double tax, double groceries, double waterlights, double travel, double phone, double other)
        {
            try
            {
                if (income <= 0 || tax <= 0 || groceries <= 0 || waterlights <= 0)
                {
                    ViewBag.Error = "Values cannot be 0";
                    return View();
                }
                else if (income <=  tax)
                {
                    ViewBag.Error = "Tax cannot be more than your gross income";
                    return View();
                }
                else
                {
                    GeneralExpense.GrossIncome = income;
                    GeneralExpense.TaxDeducted = tax;
                    GeneralExpense.Groceries = groceries;
                    GeneralExpense.WaterLights = waterlights;
                    GeneralExpense.TravelCosts = travel;
                    GeneralExpense.PhoneCosts = phone;
                    GeneralExpense.OtherExpenses = other;

                    return RedirectToAction("Home", "Data");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Home()
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    if (GeneralExpense.GrossIncome <= 0)
                    {
                        TempData["GeneralFirst"] = "You need to fill in General Expenses before continuing.";
                        return RedirectToAction("General", "Data");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Home(double purchase, double deposit, double interest, double monthsrepay, double monthlyRent)
        {
            try
            {
                if (monthlyRent != 0)
                {
                    monthsrepay = 0;
                }
                if (deposit <= purchase)
                {
                    HomeLoan homeLoan = new HomeLoan(purchase, deposit, interest, monthsrepay);
                    RentalExpense rentalExpense = new RentalExpense(monthlyRent);

                    return RedirectToAction("Car", "Data");
                }
                else
                {
                    ViewBag.Error = "Deposit cannot be more than the purchase price!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Car()
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    if (GeneralExpense.GrossIncome <= 0)
                    {
                        TempData["GeneralFirst"] = "You need to fill in General Expenses before continuing.";
                        return RedirectToAction("General", "Data");
                    }
                    else if (HomeLoan.HomePurchasePrice == 0 && RentalExpense.MonthlyRental == 0)
                    {
                        TempData["HomeFirst"] = "You need to fill in Home Expenses before continuing.";
                        return RedirectToAction("Home", "Data");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Car(string carmodelmake, double carpurchase, double cardeposit, double carinterest, double carinsurance)
        {
            //REMEMBER TO RESET LIBRARY STATS BACK TO 0
            try
            {
                if (cardeposit <= carpurchase)
                {
                    CarLoan carLoan = new CarLoan(carmodelmake, carpurchase, cardeposit, carinterest, carinsurance);

                    var user = _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefault();
                    int userID = user.UsersId;

                    var carSummary = new CarLoan();
                    var homeSummary = new HomeLoan();
                    var rentalSummary = new RentalExpense();

                    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));

                    conn.Open();

                    //This query adds the car to the car table in the database
                    string queryAddCar = "INSERT INTO BUY_CAR VALUES (" + userID + ", '" + CarLoan.CarModelMake + "', " + CarLoan.CarPurchasePrice + ", " + CarLoan.CarDeposit + ", " + CarLoan.CarInterestRate + ", " + CarLoan.CarInsurancePremium + ", " + carSummary.CarLoanRepaymentTotal() + ", " + carSummary.CarLoanRepaymentMonthly() + ");";

                    //This query adds the home loan values to the rent and buy table in the database
                    string queryAddHome = "INSERT INTO RENT_BUY_PROPERTY VALUES (" + userID + ", " + RentalExpense.MonthlyRental + ", " + HomeLoan.HomePurchasePrice + ", " + HomeLoan.HomeTotalDeposit + ", " + HomeLoan.HomeInterestRate + ", " + HomeLoan.HomeMonthsRepay + ", " + homeSummary.TotalHomeLoanRepayment() + ", " + homeSummary.MonthlyHomeLoanRepayment() + ");";

                    //This query adds all general expenses to the general expenses table
                    string queryAddGeneral = "INSERT INTO GENERAL_EXPENSES VALUES (" + userID + ", " + GeneralExpense.GrossIncome + ", " + GeneralExpense.TaxDeducted + ", " + GeneralExpense.Groceries + ", " + GeneralExpense.WaterLights + ", " + GeneralExpense.TravelCosts + ", " + GeneralExpense.PhoneCosts + ", " + GeneralExpense.OtherExpenses + ");";

                    //This is the total costs table query that recieves most of the methods created by the library 
                    string queryAddCosts = "INSERT INTO COST VALUES (" + userID + ", " + GeneralExpense.NormalExpenses() + ", " + GeneralExpense.FinalIncome() + ", " + GeneralExpense.AvailableMoneyAfterDeductions() + ", " + GeneralExpense.SpendableIncome() + ");";

                    
                        //inserting the query add car
                        SqlCommand commandTwo = new SqlCommand(queryAddCar, conn);
                        SqlDataReader dataReaderTwo = commandTwo.ExecuteReader();
                        commandTwo.Dispose();
                        dataReaderTwo.Close();

                        //inserting the query add home
                        SqlCommand commandThree = new SqlCommand(queryAddHome, conn);
                        SqlDataReader dataReaderThree = commandThree.ExecuteReader();
                        commandThree.Dispose();
                        dataReaderThree.Close();

                        //inserting the query add general expenses
                        SqlCommand commandFour = new SqlCommand(queryAddGeneral, conn);
                        SqlDataReader dataReaderFour = commandFour.ExecuteReader();
                        commandFour.Dispose();
                        dataReaderFour.Close();

                        //inserting the query add costs
                        SqlCommand commandFive = new SqlCommand(queryAddCosts, conn);
                        SqlDataReader dataReaderFive = commandFive.ExecuteReader();
                        commandFive.Dispose();
                        dataReaderFive.Close();
                        conn.Close();
                    

                    //Resetting these back to 0 so that other views cannot be accessed pre-maturely
                    var carReset = new CarLoan("", 0, 0, 0, 0);
                    var homeReset = new HomeLoan(0, 0, 0, 0);
                    var rentalReset = new RentalExpense(0);
                    GeneralExpense.GrossIncome = 0;
                    GeneralExpense.TaxDeducted = 0;
                    GeneralExpense.Groceries = 0;
                    GeneralExpense.WaterLights = 0;
                    GeneralExpense.TravelCosts = 0;
                    GeneralExpense.PhoneCosts = 0;
                    GeneralExpense.OtherExpenses = 0;

                    TempData["DataCaptured"] = "Data has been captured!";
                    return RedirectToAction("SummaryData", "Data");
                }
                else
                {
                    ViewBag.Error = "Car Deposit cannot be more than the purchase price!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }
        
        [HttpGet]
        public IActionResult Saving()
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    return View();
                }
                else
                {
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Saving(double amount, double years, string reason, double interest)
        {
            try
            {
                if (amount <= 0 || years <= 0 || interest <= 0)
                {
                    ViewBag.Error = "Values cannot be lower than or equal to 0. Please try again!";
                    return View();
                }

                Saving saving = new Saving(amount, years, reason, interest);

                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));
                conn.Open();

                var user = _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefault();
                int userID = user.UsersId;

                string query = "INSERT INTO SAVINGS VALUES ("+ userID +", "+amount+", "+Convert.ToInt32(years)+", '"+reason+"', "+Convert.ToInt32(interest)+", "+saving.MonthsToSave()+");";

                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader dataReader = command.ExecuteReader();

                conn.Close();
                command.Dispose();
                dataReader.Close();

                return RedirectToAction("SaveList", "Data");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }  
        }

        [HttpGet]
        public IActionResult SaveList()
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    var user = _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefault();
                    int userID = user.UsersId;

                    var history = _context.Savings.Where(x => x.UsersId == userID).FirstOrDefault();
                    if (history == null)
                    {
                        ViewBag.History = "No History!";
                    }

                    return View(_context.Savings.Where(x => x.UsersId.Equals(userID)));
                }
                else
                {
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.History = "Error: " + ex.Message;
                return View();
            }
        }
    }
}

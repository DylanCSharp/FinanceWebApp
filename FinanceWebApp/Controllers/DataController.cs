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
using Microsoft.EntityFrameworkCore;

namespace FinanceWebApp.Controllers
{
    public class DataController : Controller
    {

        readonly FinanceApplicationContext _context;
        readonly IConfiguration _configuration;

        //Instatiating the dbcontext so that we can read from it.
        //Instatiating the Iconfiguration import so that we can get our connection string without hardcoding it
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
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    return View();
                }
                else
                {
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult General(double income, double tax, double groceries, double waterlights, double travel, double phone, double other)
        {
            //FOR POST METHODS WE DO NOT HAVE TO CHECK IF THE USER IS LOGGED IN, 
            //BECAUSE IN ORDER TO GET TO POST METHOD, THEY HAVE TO BE LOGGED IN, THEREFORE WE DONT WASTE TIME CHECKING IF THE USER IS LOGGED IN FOR POST METHODS
            try
            {
                //Making sure that the user enters the correct values and that they make sense
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
                    //Setting the abstract class fields to their specific values so that calculations can be made
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
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Home()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
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
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Home(double purchase, double deposit, double interest, double monthsrepay, double monthlyRent)
        {
            //FOR POST METHODS WE DO NOT HAVE TO CHECK IF THE USER IS LOGGED IN, 
            //BECAUSE IN ORDER TO GET TO POST METHOD, THEY HAVE TO BE LOGGED IN, THEREFORE WE DONT WASTE TIME CHECKING IF THE USER IS LOGGED IN FOR POST METHODS
            try
            {
                if (monthlyRent != 0)
                {
                    monthsrepay = 0;
                }
                if (deposit <= purchase)
                {
                    //Setting the library values to the values recieved from the view
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
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Car()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    //Making sure the user has entered values for the previous page, this is so that an accurate write to the database is made
                    if (GeneralExpense.GrossIncome <= 0)
                    {
                        TempData["GeneralFirst"] = "You need to fill in General Expenses before continuing.";
                        return RedirectToAction("General", "Data");
                    }
                    //Making sure the user has entered values for the previous page, this is so that an accurate write to the database is made
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
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Car(string carmodelmake, double carpurchase, double cardeposit, double carinterest, double carinsurance)
        {
            //FOR POST METHODS WE DO NOT HAVE TO CHECK IF THE USER IS LOGGED IN, 
            //BECAUSE IN ORDER TO GET TO POST METHOD, THEY HAVE TO BE LOGGED IN, THEREFORE WE DONT WASTE TIME CHECKING IF THE USER IS LOGGED IN FOR POST METHODS


            //MAKING USE OF ASYNCHRONOUS TASKS SO THAT WHEN A THREAD IS AVAILABLE IT CAN EXECUTE THE NEXT BIT OF CODE
            try
            {
                if (cardeposit <= carpurchase)
                {
                    CarLoan carLoan = new CarLoan(carmodelmake, carpurchase, cardeposit, carinterest, carinsurance);

                    var user = await _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefaultAsync();
                    int userID = user.UsersId;

                    var carSummary = new CarLoan();
                    var homeSummary = new HomeLoan();
                    var rentalSummary = new RentalExpense();

                    SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));

                    await conn.OpenAsync();

                    //This query adds the car to the car table in the database
                    string queryAddCar = "INSERT INTO BUY_CAR (USERS_ID, CAR_MAKE, CAR_PURCHASE, CAR_DEPOSIT, CAR_INTEREST, CAR_INSURANCE, TOTAL_CAR_REPAYMENT, MONTHLY_CAR_REPAYMENT) VALUES (" + userID + ", '" + carmodelmake + "', " + carpurchase + ", " + cardeposit + ", " + carinterest + ", " + carinsurance + ", '" + Math.Round(carSummary.CarLoanRepaymentTotal(),2) + "', '" +Math.Round(carSummary.CarLoanRepaymentMonthly(),2) + "');";

                    //This query adds the home loan values to the rent and buy table in the database
                    string queryAddHome = "INSERT INTO RENT_BUY_PROPERTY (USERS_ID, RENT_MONTHLY, PROPERTY_PURCHASE, PROPERTY_DEPOSIT, PROPERTY_INTEREST, PROPERTY_MONTHSREPAY, TOTAL_HOME_REPAYMENT, MONTHLY_HOME_REPAYMENT) VALUES(" + userID + ", " + RentalExpense.MonthlyRental + ", " + HomeLoan.HomePurchasePrice + ", " + HomeLoan.HomeTotalDeposit + ", " + HomeLoan.HomeInterestRate + ", " + HomeLoan.HomeMonthsRepay + ", '" + Math.Round(homeSummary.TotalHomeLoanRepayment(),2) + "', '" + Math.Round(homeSummary.MonthlyHomeLoanRepayment(),2) + "'); ";

                    //This query adds all general expenses to the general expenses table
                    string queryAddGeneral = "INSERT INTO GENERAL_EXPENSES (USERS_ID, GROSS_INCOME, TAX_DEDUCTED, GROCERIES, WATER_LIGHTS, TRAVEL, PHONE, OTHER) VALUES (" + userID + ", " + GeneralExpense.GrossIncome + ", " + GeneralExpense.TaxDeducted + ", " + GeneralExpense.Groceries + ", " + GeneralExpense.WaterLights + ", " + GeneralExpense.TravelCosts + ", " + GeneralExpense.PhoneCosts + ", " + GeneralExpense.OtherExpenses + ");";
                    //This is the total costs table query that recieves most of the methods created by the library 
                    string queryAddCosts = "INSERT INTO COST (USERS_ID, NORMAL_EXPENSES, FINAL_INCOME, POST_DEDUCTIONS, SPENDABLE_INCOME) VALUES (" + userID + ", '" + Math.Round(GeneralExpense.NormalExpenses(),2) + "', '" + Math.Round(GeneralExpense.FinalIncome(),2) + "', '" + Math.Round(GeneralExpense.AvailableMoneyAfterDeductions(),2) + "', '" + Math.Round(GeneralExpense.SpendableIncome(),2) + "');";


                    //MAKING USE OF ASYNCHRONOUS TASKS SO THAT WHEN A THREAD IS AVAILABLE IT CAN EXECUTE THE NEXT BIT OF CODE
                    //inserting the query add car
                    SqlCommand commandTwo = new SqlCommand(queryAddCar, conn);
                    SqlDataReader dataReaderTwo = await commandTwo.ExecuteReaderAsync();
                    await commandTwo.DisposeAsync();
                    await dataReaderTwo.CloseAsync();

                    //inserting the query add home
                    SqlCommand commandThree = new SqlCommand(queryAddHome, conn);
                    SqlDataReader dataReaderThree = await commandThree.ExecuteReaderAsync();
                    await commandThree.DisposeAsync();
                    await dataReaderThree.CloseAsync();

                    //inserting the query add general expenses
                    SqlCommand commandFour = new SqlCommand(queryAddGeneral, conn);
                    SqlDataReader dataReaderFour = await commandFour.ExecuteReaderAsync();
                    await commandFour.DisposeAsync();
                    await dataReaderFour.CloseAsync();

                    //inserting the query add costs
                    SqlCommand commandFive = new SqlCommand(queryAddCosts, conn);
                    SqlDataReader dataReaderFive = await commandFive.ExecuteReaderAsync();
                    await commandFive.DisposeAsync();
                    await dataReaderFive.CloseAsync();

                    //Closing the database connection
                    await conn.CloseAsync();


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

                    TempData["DataCaptured"] = "New Data has been captured!";
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
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }
        
        [HttpGet]
        public IActionResult Saving()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    return View();
                }
                else
                {
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Saving(double amount, double years, string reason, double interest)
        {
            //FOR POST METHODS WE DO NOT HAVE TO CHECK IF THE USER IS LOGGED IN, 
            //BECAUSE IN ORDER TO GET TO POST METHOD, THEY HAVE TO BE LOGGED IN, THEREFORE WE DONT WASTE TIME CHECKING IF THE USER IS LOGGED IN FOR POST METHODS
            try
            {
                if (amount <= 0 || years <= 0 || interest <= 0)
                {
                    ViewBag.Error = "Values cannot be lower than or equal to 0. Please try again!";
                    return View();
                }

                Saving saving = new Saving(amount, years, reason, interest);


                //MAKING USE OF ASYNCHRONOUS TASKS SO THAT WHEN A THREAD IS AVAILABLE IT CAN EXECUTE THE NEXT BIT OF CODE
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));
                await conn.OpenAsync();

                var user = _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefault();
                int userID = user.UsersId;

                string query = "INSERT INTO SAVINGS (USERS_ID, SAVING_AMOUNT, SAVING_YEARS, SAVING_REASON, SAVING_INTERESTRATE, MONTHLY_AMOUNT_TOSAVE) VALUES (" + userID + ", " + amount + ", " + years + ", '" + reason + "', "+interest+", '"+Math.Round(saving.MonthsToSave(),2)+"');";

                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Closing all asynchronous connections to the database
                await conn.CloseAsync();
                await command.DisposeAsync();
                await dataReader.CloseAsync();

                return RedirectToAction("SaveList", "Data");
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }  
        }

        [HttpGet]
        public async Task<IActionResult> SaveList()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    var user = await _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefaultAsync();
                    int userID = user.UsersId;

                    //Checking if the user has history of data logs
                    var history = await _context.Savings.Where(x => x.UsersId == userID).FirstOrDefaultAsync();
                    if (history == null)
                    {
                        ViewBag.History = "No History!";
                    }

                    return View(_context.Savings.Where(x => x.UsersId.Equals(userID)));
                }
                else
                {
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.History = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> SummaryData()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    var user = await _context.Users.Where(x => x.Email.Equals(HttpContext.Session.GetString("LoggedInUser"))).FirstOrDefaultAsync();
                    int userID = user.UsersId;

                    //Checking if the user has history of data logs
                    var history = await _context.Cost.Where(x => x.UsersId == userID).FirstOrDefaultAsync();
                    if (history == null)
                    {
                        ViewBag.History = "No History!";
                    }

                    return View(_context.Cost.Where(x => x.UsersId == userID));
                }
                else
                {
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error:" + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult SummaryData(int? id)
        {
            //FOR POST METHODS WE DO NOT HAVE TO CHECK IF THE USER IS LOGGED IN, 
            //BECAUSE IN ORDER TO GET TO POST METHOD, THEY HAVE TO BE LOGGED IN, THEREFORE WE DONT WASTE TIME CHECKING IF THE USER IS LOGGED IN FOR POST METHODS
            try
            {
                TempData["SummaryID"] = id;
                return RedirectToAction("SummaryFinal", "Data");
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error:" + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult SummaryFinal()
        {
            try
            {
                //Making sure the user is logged in before having access to this view
                if (HttpContext.Session.GetString("LoggedInUser") != null)
                {
                    int id = Convert.ToInt32(TempData["SummaryID"]);
                    return View(_context.Cost.Where(x => x.CostsId == id));
                }
                else
                {
                    //Redirecting the user to the login page so that they can access the views
                    TempData["LoginFirst"] = "You have not logged in";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                //Catching exceptions we might have not catered for in our code
                ViewBag.Error = "Error:" + ex.Message;
                return View();
            }
        }
    }
}

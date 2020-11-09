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
                HomeLoan homeLoan = new HomeLoan(purchase, deposit, interest, monthsrepay);
                RentalExpense rentalExpense = new RentalExpense(monthlyRent);

                return RedirectToAction("Car", "Data");
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
        public IActionResult Car(string modelmake,double purchase, double deposit, int? interest, double insurance)
        {
            //REMEMBER TO RESET LIBRARY STATS BACK TO 0
            try
            {
                return RedirectToAction("Car", "Data");
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

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

        [HttpPost]
        public IActionResult General(double income, double tax, double groceries, double waterlights, double other, double travel, double phone)
        {
            return RedirectToAction("Home", "Data");
        }

        [HttpGet]
        public IActionResult Home()
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

        [HttpPost]
        public IActionResult Home(double purchase, double deposit, int? interest, int monthsrepay)
        {
            return RedirectToAction("Car", "Data"); 
        }

        [HttpGet]
        public IActionResult Car()
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

        [HttpPost]
        public IActionResult Car(string modelmake,double purchase, double deposit, int? interest, double insurance)
        {
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

        [HttpPost]
        public IActionResult Saving(double amount, double years, string reason, double interest)
        {
            try
            {
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
    }
}

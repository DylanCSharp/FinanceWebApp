using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceLibrary;

namespace FinanceWebApp.Controllers
{
    public class DataController : Controller
    {
        [HttpGet]
        public IActionResult General()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                return View();
            }
            else
            {
                ViewBag.Error = "You have not logged in";
                return View();
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
                ViewBag.Error = "You have not logged in";
                return View();
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
                ViewBag.Error = "You have not logged in";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Car(string modelmake,double purchase, double deposit, int? interest, double insurance)
        {
            return RedirectToAction("Car", "Data");
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
                ViewBag.Error = "You have not logged in";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Saving(double amount, double years, string reason, double interest)
        {
            Saving saving = new Saving(amount, years, reason, interest);
            ViewBag.Error = saving.MonthsToSave().ToString();
            return View();
        }
    }
}

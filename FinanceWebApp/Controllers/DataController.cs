using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FinanceWebApp.Controllers
{
    public class DataController : Controller
    {
        [HttpGet]
        public IActionResult General()
        {
            return View();
        }

        [HttpPost]
        public IActionResult General(double income, double tax, double groceries, double waterlights, double other, double travel, double phone)
        {
            return RedirectToAction("Home", "Data");
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Home(double purchase, double deposit, int? interest, int monthsrepay)
        {
            return RedirectToAction("Car", "Data");
        }

        [HttpGet]
        public IActionResult Car()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Car(string modelmake,double purchase, double deposit, int? interest, double insurance)
        {
            return RedirectToAction("Car", "Data");
        }
    }
}

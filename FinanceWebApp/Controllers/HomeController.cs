using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FinanceWebApp.Models;

namespace FinanceWebApp.Controllers
{
    public class HomeController : Controller
    {
        //Showing the home page of the application
        public IActionResult Index()
        {
            return View();
        }
    }
}

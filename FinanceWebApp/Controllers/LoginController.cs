using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Scrypt;

namespace FinanceWebApp.Controllers
{

    public class LoginController : Controller
    {

        readonly FinanceApplicationContext _context;
        readonly IConfiguration _configuration;

        public LoginController(FinanceApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(string email, string password, string fullname, string passwordconfirm, string phone)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            try
            {
                if (email != "" || password != "" || fullname != "" || phone != "")
                {
                    var usernameValid = _context.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();

                    if (password.Contains(passwordconfirm))
                    {
                        if (usernameValid == null)
                        {
                            string hashedPassword = encoder.Encode(password);

                            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));
                            conn.Open();
                            string query = "INSERT INTO USERS VALUES ('" + fullname + "', '" + email + "', '" + hashedPassword + "', '" + phone + "');";

                            SqlCommand command = new SqlCommand(query, conn);
                            SqlDataReader dataReader = command.ExecuteReader();

                            conn.Close();
                            command.Dispose();
                            dataReader.Close();

                            TempData["Registered"] = "You have been successfully registered! Welcome to Finance Expert " + fullname + " !";
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            ViewBag.Error = "Username already exists in our database.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Passwords do not match!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "Values need to be entered! Try again!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }
    }
}

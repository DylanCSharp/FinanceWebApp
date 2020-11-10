using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                //Telling the user they are alreadt logged in if they access this page after they log in
                ViewBag.LoggedIn = "You are already logged in.";
                return View();
            }
            else
            {
                return View();
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            //Using the libray that helps with hashing and salting Scrypt.NET Nuget Package Added
            ScryptEncoder encoder = new ScryptEncoder();

            try
            {
                //Checking if the user exists
                var user = await _context.Users.Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();

                if (user != null)
                {
                    //Checks to see if the hashed and salted value in the database is valid with the password entered with the built in Compare() method in the library
                    bool validUser = encoder.Compare(password, user.UserPassword);

                    if (validUser == true)
                    {
                        //Showing the user they have logged in
                        TempData["LoggedIn"] = user.UserFullname + ", you are logged in.";
                        HttpContext.Session.SetString("LoggedInUser", email);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //Showing the user they have entered invalid credentials for the username
                        ViewBag.Error = "Username or password is invalid!";
                        return View();
                    }  
                }
                else
                {
                    //Telling the user that this user does not exist if they entered a invalid username
                    ViewBag.Error = "User does not exist!";
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
        public IActionResult RegisterUser()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                //If the user tries to register while logged in, they will be told they are already logged in
                ViewBag.Error = "You are already logged in.";
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(string email, string password, string fullname, string passwordconfirm, string phone)
        {
            //Using the libray that helps with hashing and salting Scrypt.NET Nuget Package Added
            ScryptEncoder encoder = new ScryptEncoder();

            //MAKING USE OF ASYNCHRONOUS AND AWAIT TO MAXIMISE PERFORMANCE AND SCALABILITY
            try
            {
                
                //Making sure all values are entered
                if (email != "" || password != "" || fullname != "" || phone != "")
                {
                    //Checking if the password contains the other password so that the passwords are the same
                    if (password.Contains(passwordconfirm))
                    {
                        //Checking if the user already exists within the database
                        var usernameValid = await _context.Users.Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();

                        //Executing this if statement if the user doesnt exist
                        if (usernameValid == null)
                        {
                            //Hasing and salting the password with the library built in method Encode();
                            string hashedPassword = encoder.Encode(password);

                            //Opening up a connection to the database and getting the connection string
                            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("FinanceDatabase"));
                            await conn.OpenAsync();

                            //Query for the database to execute, inserting the hashed and salted password 
                            string query = "INSERT INTO USERS VALUES ('" + fullname + "', '" + email + "', '" + hashedPassword + "', '" + phone + "');";

                            SqlCommand command = new SqlCommand(query, conn);
                            SqlDataReader dataReader = await command.ExecuteReaderAsync();

                            //Closing the connections to database
                            await conn.CloseAsync();
                            await command.DisposeAsync();
                            await dataReader.CloseAsync();

                            //Showing the user they have logged in
                            TempData["Registered"] = "You have been successfully registered! Welcome to Finance Expert " + fullname + " !";
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            //Showing the user that the username already exists
                            ViewBag.Error = "User already exists in our database.";
                            return View();
                        }
                    }
                    else
                    {
                        //Showing the user that the passwords do not match
                        ViewBag.Error = "Passwords do not match!";
                        return View();
                    }
                }
                else
                {
                    //Showing the user that all values must be entered 
                    ViewBag.Error = "Values need to be entered! Try again!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                //Displaying any other exceptions that werent handled by code
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            try
            {
                //Clearing the session variable so that another person can log in to another account
                HttpContext.Session.Clear();
                TempData["LoggedOut"] = "You have been logged out!";
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                TempData["LoggedOut"] = "Error: " + ex.Message;
                return RedirectToAction("Login", "Login");
            }
        }
    }
}

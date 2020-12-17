using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWall.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace TheWall.Controllers
{
    public class LoginController : Controller
    {
        private Context dbcontext;
        public LoginController(Context context)
        {
            dbcontext = context;
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet("register")]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost("createUser")]
        public IActionResult CreateUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (dbcontext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View("Registration");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                    dbcontext.Users.Add(newUser);
                    dbcontext.SaveChanges();
                    HttpContext.Session.SetObjectAsJson("LoggedUser", dbcontext.Users.FirstOrDefault(u => u.Email == newUser.Email));
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return View("Registration");
            }
        }
        [HttpPost("loginattempt")]
        public IActionResult LoginAttempt(LoginUser logged)
        {
            if (ModelState.IsValid)
            {
                User dbuser = dbcontext.Users.FirstOrDefault(u => u.Email == logged.Email);
                if (dbuser != null)
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(logged, dbuser.Password, logged.Password);
                    if (result != 0)
                    {
                        HttpContext.Session.SetObjectAsJson("LoggedUser", dbuser);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("Email", "Invalid Email or Password");
            }
            return View("Login");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
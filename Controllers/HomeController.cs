using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWall.Models;
using Microsoft.EntityFrameworkCore;

namespace TheWall.Controllers
{
    public class HomeController : Controller
    {
        private Context dbcontext;
        public HomeController(Context context)
        {
            dbcontext = context;
        }
        public User Logged()
        {
            User logged = HttpContext.Session.GetObjectFromJson<User>("LoggedUser");
            return logged;
        }
        public IActionResult Index()
        {
            User logged = this.Logged();
            if(logged == null)
            {
                return RedirectToAction("Login","Login");
            }
            ViewBag.Messages = dbcontext.Messages.Include(m => m.Creator).OrderByDescending(m => m.CreatedAt).ToList();
            ViewBag.Comments = dbcontext.Comments.OrderBy(c => c.CreatedAt).Include(c => c.User).Include(c => c.Message);
            ViewBag.User = logged;
            return View();
        }

        public IActionResult SendMessage(Message newM)
        {
            if(ModelState.IsValid)
            {
                newM.UserID = this.Logged().UserID;
                dbcontext.Add(newM);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult SendComment(Comment newC)
        {
            if(ModelState.IsValid)
            {
                newC.UserId = this.Logged().UserID;
                dbcontext.Add(newC);
                dbcontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet("/delete/{messageid}")]
        public IActionResult DeleteMessage (int messageid)
        {
            Message toDelete = dbcontext.Messages.FirstOrDefault(m => m.MessageID == messageid);
            dbcontext.Remove(toDelete);
            dbcontext.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

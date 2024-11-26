using System.Diagnostics;
using IBLab.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBLab.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }


    }
}

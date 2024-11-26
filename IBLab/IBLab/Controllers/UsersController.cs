using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IBLab.Data;
using IBLab.Models;
using IBLab.Service.interfaces;

namespace IBLab.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService UserService)
        {
            _userService = UserService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Register");
        }

       
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Verification()
        {
            return View();
        }

        public IActionResult TFA()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verification(string code)
        {
            try
            {
                _userService.ValidateOTP(HttpContext.Session.GetString("TempUsername"), code);
                _userService.CreateUser(HttpContext.Session.GetString("TempUsername"));
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }  
        }

        [HttpPost]
        public IActionResult TFA(string code)
        {
            try
            {
                _userService.ValidateOTP(HttpContext.Session.GetString("TempUsername"), code);
                HttpContext.Session.SetString("Username", HttpContext.Session.GetString("TempUsername"));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                
                _userService.ValidateLogin(username, password);
                HttpContext.Session.SetString("TempUsername", username);
                _userService.TFA(HttpContext.Session.GetString("TempUsername"));
                return RedirectToAction("TFA");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }

        }

        [HttpPost]
        public IActionResult Register(string email, string username, string password)
        {
            try
            {
                _userService.CreateTempUser(email, username, password);
                HttpContext.Session.SetString("TempUsername", username);
                return RedirectToAction("Verification");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Users");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Dal.DataModel;
using TicTacToe.Services.Interfaces;

namespace TicTacToe.WebUI.Areas.Registration.Controllers
{
    [Area("Registration")]
    public class UserRegistrationController : Controller
    {
        private IUserService userService;

        public UserRegistrationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                await userService.RegisterUser(user);
                return RedirectToAction(nameof(EmailConfirmation), new { Area = "Registration", Controller="UserRegistration",  Email = user.Email });
            }
            else
            {
                return View(user);
            }
        }

        public async Task<IActionResult> EmailConfirmation(string email)
        {
            var user = await userService.GetUserByEmail(email);
            if (user?.IsEmailConfirmed == true)
            {
                return RedirectToAction("Index", "GameInvitation", new { Area="Registration", Email = email });
            }
            ViewBag.Email = email;
            return View();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.WebUI.Areas.Registration.Controllers
{
    [Area("Registration")]
    [assembly: ResourceLocation("~/Localization/Registration")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var culture = Request.HttpContext.Session.GetString("culture");
            ViewBag.Language = culture;
            return View();
        }

        public IActionResult SetCulture(string culture)
        {
            Request.HttpContext.Session.SetString("culture", culture);
            return RedirectToAction("Index");
        }
    }
}

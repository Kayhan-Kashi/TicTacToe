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
    public class GameInvitationController : Controller
    {

        private IUserService userService;

        public GameInvitationController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index(string email)
        {
            var gameInvitation = new GameInvitation
            {
                InvitedBy = email
            };
            return View(gameInvitation);
        }
    }
}

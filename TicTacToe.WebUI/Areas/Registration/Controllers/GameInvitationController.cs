using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicTacToe.Dal.DataModel;
using TicTacToe.Services.Interfaces;

namespace TicTacToe.WebUI.Areas.Registration.Controllers
{
    [Area("Registration")]
    public class GameInvitationController : Controller
    {

        private IUserService userService;
        private IStringLocalizer<GameInvitationController> stringLocalizer;

        public GameInvitationController(IUserService userService, IStringLocalizer<GameInvitationController> stringLocalizer)
        {
            this.userService = userService;
            this.stringLocalizer = stringLocalizer;
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

        [HttpPost]

        public IActionResult Index(GameInvitation gameInvitation)
        {
            return Content(stringLocalizer["GameInvitationConfirmationMessage", gameInvitation.EmailTo]);
        }

    }
}

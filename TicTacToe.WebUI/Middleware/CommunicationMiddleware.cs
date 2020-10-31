using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Services.Interfaces;

namespace TicTacToe.WebUI.Middleware
{
    public class CommunicationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IUserService userService;

        public CommunicationMiddleware(RequestDelegate next, IUserService userService)
        {
            this.next = next;
            this.userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Equals("/Registration/CheckEmailConfirmationStatus"))
            {
                await ProcessEmailConfirmation(context);  
            }
            else
            {
                await next?.Invoke(context);
            }
        }

        private async Task ProcessEmailConfirmation(HttpContext context)
        {
            var email = context.Request.Query["email"];
            var user =  await userService.GetUserByEmail(email);

            if (string.IsNullOrEmpty(email))
            {
                await context.Response.WriteAsync("BadRequest:Email is required.");
            }
            else if ((await userService.GetUserByEmail(email)).IsEmailConfirmed)
            {
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await context.Response.WriteAsync("WaitingForEmailConfirmation");
                user.IsEmailConfirmed = true;
                user.EmailConfirmationDate = DateTime.Now;
                await userService.UpdateUser(user);
            }
        }
    }
}

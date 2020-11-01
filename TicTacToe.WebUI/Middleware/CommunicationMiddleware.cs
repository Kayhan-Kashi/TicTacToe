using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
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
            if (context.WebSockets.IsWebSocketRequest)
            {
                var currentSocket = await context.WebSockets.AcceptWebSocketAsync();
                string json = await RecieveStringAsync(currentSocket, context.RequestAborted);
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                switch (data.Operation.ToString())
                {
                    case "CheckEmailConfirmationStatus":
                        await ProcessEmailConfirmation(context, currentSocket, context.RequestAborted, data.Parameters.ToString());
                        break;
                }
            }
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

        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
            return socket.SendAsync(buffer, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> RecieveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            WebSocketReceiveResult result;
            using(var ms = new MemoryStream())
            {
                do
                {
                    ct.ThrowIfCancellationRequested();
                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if(result.MessageType != WebSocketMessageType.Text)
                    throw new Exception("Unexpected message");
         
                using(var streamReader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        public async Task ProcessEmailConfirmation(HttpContext context, WebSocket currentSocket, CancellationToken ct, string email)
        {
            var user = await userService.GetUserByEmail(email);
            while(!ct.IsCancellationRequested && !currentSocket.CloseStatus.HasValue && user?.IsEmailConfirmed == false)
            {
                if (user.IsEmailConfirmed)
                    await SendStringAsync(currentSocket, "OK", ct);
                else
                {
                    user.IsEmailConfirmed = true;
                    user.EmailConfirmationDate = DateTime.Now;
                    await userService.UpdateUser(user);
                    await SendStringAsync(currentSocket, "OK", ct);
                }

                Task.Delay(500).Wait();
                user = await userService.GetUserByEmail(email);
            }
        }
    }
}

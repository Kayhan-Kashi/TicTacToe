using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Dal.DataModel;
using TicTacToe.Services.Interfaces;

namespace TicTacToe.Services.Concretes
{
    public class UserService : IUserService
    {
        private static ConcurrentBag<User> userStore;

        static UserService()
        {
            userStore = new ConcurrentBag<User>() 
            { 
                new User { Email = "Kayhan_Kashi@yahoo.com", FirstName = "Kayhan", LastName = "Kashi" } 
            };
        }

        public Task<bool> RegisterUser(User user)
        {
            return Task.FromResult(true);
        }
        public Task<bool> IsOnline(string name)
        {
            return Task.FromResult(true);
        }
        public Task<User> GetUserByEmail(string email)
        {
            return Task.FromResult(
                userStore.Where(u => u.Email.Equals(email)).FirstOrDefault()
                );
        }
        public Task UpdateUser(User user)
        {
            userStore = new ConcurrentBag<User>(userStore.Where(u => u.Email != user.Email)) { user };
            return Task.CompletedTask;
        }
    }
}

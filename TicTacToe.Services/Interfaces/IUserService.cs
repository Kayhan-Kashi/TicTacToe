using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Dal.DataModel;

namespace TicTacToe.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsOnline(string name);
        Task<bool> RegisterUser(User user);
        Task<User> GetUserByEmail(string email);
        Task UpdateUser(User user);
    }
}

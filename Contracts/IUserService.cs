using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task RegisterUser(User user);
    }
}

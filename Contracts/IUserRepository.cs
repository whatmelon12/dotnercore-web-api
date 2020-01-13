using System;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}

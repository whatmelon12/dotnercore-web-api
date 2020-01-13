using System;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateUser(User user)
        {
            Create(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await FindByCondition(user => user.Id.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await FindByCondition(user => user.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}

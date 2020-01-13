using System;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;

namespace UserService
{
    public class UserService : IUserService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(ILoggerManager logger, IRepositoryWrapper repository, IPasswordHasher passwordHasher)
        {
            _logger = logger;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _repository.User.GetUserByEmailAsync(email);

            if(_passwordHasher.Check(user.Password, password))
            {
                return user;
            }

            _logger.LogWarn($"Authentication attempt failed for user email: {email}");

            return null;
        }

        public async Task RegisterUser(User user)
        {
            var existingUser = await _repository.User.GetUserByEmailAsync(user.Email);

            if(existingUser != null)
            {
                throw new InvalidOperationException("Email already in use");
            }

            user.Password = _passwordHasher.Hash(user.Password);

            _repository.User.CreateUser(user);

            await _repository.SaveAsync();
        }
    }
}

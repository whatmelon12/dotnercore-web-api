using System;
using System.Threading.Tasks;
using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;
        private IUserRepository _user;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IOwnerRepository Owner {
            get
            {
                if(_owner == null)
                {
                    _owner = new OwnerRepository(_repositoryContext);
                }

                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if(_account == null)
                {
                    _account = new AccountRepository(_repositoryContext);
                }

                return _account;
            }
        }

        public IUserRepository User
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_repositoryContext);
                }

                return _user;
            }
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }
    }
}

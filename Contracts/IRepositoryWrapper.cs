using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IOwnerRepository Owner { get; }
        IAccountRepository Account { get; }
        IUserRepository User { get;  }
        Task SaveAsync();
    }
}

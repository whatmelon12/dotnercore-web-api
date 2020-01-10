using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync(int page, int limit);
        Task<Owner> GetOwnerByIdAsync(Guid ownerId);
        Task<IEnumerable<Owner>> GetTopOwnersAsync();
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
    }
}

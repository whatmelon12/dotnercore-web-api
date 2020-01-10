using System;
using System.Collections.Generic;
using Entities.Models;

namespace Contracts
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        IEnumerable<Owner> GetAllOwners(int page, int limit);
        Owner GetOwnerById(Guid ownerId);
        void CreateOwner(Owner owner);
        void UpdateOwner(Owner owner);
        void DeleteOwner(Owner owner);
        IEnumerable<Owner> GetTopOwners();
    }
}

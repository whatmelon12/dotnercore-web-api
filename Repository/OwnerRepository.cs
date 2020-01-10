using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync(int page = 1, int limit = 10)
        {
            return await FindAll().OrderBy(o => o.Name).Skip((page == 1 ? 0 : page) * limit).Take(limit).ToListAsync();
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
        {
            return await FindByCondition(owner => owner.Id.Equals(ownerId)).FirstOrDefaultAsync();
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        public async Task<IEnumerable<Owner>> GetTopOwnersAsync()
        {
           return await _repositoryContext.Owners.FromSqlRaw("SELECT TOP(3) * FROM owner").ToListAsync();
        }
    }
}

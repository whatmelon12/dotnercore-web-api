using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using restfulDemo.API.ActionFilters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restfulDemo.API.Controllers
{
    [Route("api/owner")]
    public class OwnerController : Controller
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var owners = await _repository.Owner.GetAllOwnersAsync(page, limit);

            var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);

            return Ok(ownersResult);
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public async Task<IActionResult> GetOwnerById(Guid id)
        {
            var owner = await _repository.Owner.GetOwnerByIdAsync(id);

            if (owner == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");

                return NotFound();
            }

            var ownerResult = _mapper.Map<OwnerDto>(owner);

            return Ok(ownerResult);
        }

        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerCreationDto owner)
        {
            var ownerEntity = _mapper.Map<Owner>(owner);

            _repository.Owner.CreateOwner(ownerEntity);

            await _repository.SaveAsync();

            var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

            return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerUpdateDto owner)
        {
            var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);

            if (ownerEntity == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");

                return NotFound();
            }

            _mapper.Map(owner, ownerEntity);

            _repository.Owner.UpdateOwner(ownerEntity);

            await _repository.SaveAsync();

            var updatedOwner = _mapper.Map<OwnerDto>(ownerEntity);

            return Ok(updatedOwner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);

            if (ownerEntity == null)
            {
                _logger.LogError($"Owner with id: {id}, hasn't been found in db.");

                return NotFound();
            }

            _repository.Owner.DeleteOwner(ownerEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpGet("/arbitrary")]
        public async Task<IActionResult> GetTopOwners()
        {
            var owners = await _repository.Owner.GetTopOwnersAsync();

            return Ok(owners);
        }
    }
}

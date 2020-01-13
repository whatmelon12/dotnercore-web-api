using Entities.ModelExtensions;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using restfulDemo.API.ActionFilters;
using Entities.Models;
using System;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restfulDemo.API.Controllers
{
    [Route("api/user")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserController(
            IRepositoryWrapper repository,
            IUserService userService,
            IMapper mapper,
            IConfiguration config)
        {
            _repository = repository;
            _userService = userService;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> Login([FromBody] AuthCredentialsDto credentials)
        {
            var user = await _userService.AuthenticateAsync(credentials.Email, credentials.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var token = user.ToJWT(_config);

            var authenticatedUser = _mapper.Map<UserAuthenticatedDto>(user);

            authenticatedUser.Token = token;

            return Ok(authenticatedUser);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<IActionResult> Register([FromBody] UserCreationDto user)
        {
            var userEntity = _mapper.Map<User>(user);

            await _userService.RegisterUser(userEntity);

            var token = userEntity.ToJWT(_config);

            var createdUser = _mapper.Map<UserAuthenticatedDto>(userEntity);

            createdUser.Token = token;

            return CreatedAtRoute("UserById", new { id = createdUser.Id }, createdUser);
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _repository.User.GetUserByIdAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            var userResult = _mapper.Map<UserDto>(user);

            return Ok(userResult);
        }
    }
}

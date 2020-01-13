using System;
namespace Entities.DataTransferObjects
{
    public class UserAuthenticatedDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}

using System;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace restfulDemo.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerCreationDto, Owner>();
            CreateMap<OwnerUpdateDto, Owner>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserAuthenticatedDto>();
            CreateMap<UserCreationDto, User>();
        }
    }
}

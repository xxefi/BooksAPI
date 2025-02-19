using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;
using AutoMapper;

namespace Auth.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateUserCredentialsDto, User>()
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.Ignore())
            .ForMember(dest => dest.LastName, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore());

        
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        
        CreateMap<User, UserDto>();
        CreateMap<User, UpdateUserDto>();
        
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<Role, RoleDto>();
        
        CreateMap<CreateBlackListedDto, BlackListed>();
        CreateMap<BlackListed, BlackListedDto>().ReverseMap();
        
        CreateMap<CreateUserActiveSessionDto, UserActiveSessions>();
        CreateMap<UpdateUserActiveSessionDto, UserActiveSessions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UserActiveSessions, UserDto>();
    }
}
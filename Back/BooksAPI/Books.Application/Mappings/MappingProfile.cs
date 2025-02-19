using AutoMapper;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Entities;
using Books.Core.Enums;

namespace Books.Application.Mappings;

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

        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
        CreateMap<CreateBookDto, BookDto>();
        
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.BookStatusId, opt => opt.MapFrom(src => src.BookStatusId))
            .ForMember(dest => dest.BookStatus, opt => opt.MapFrom(src => src.BookStatus.ToString()))
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();
        CreateMap<Order, OrderDto>();

        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId));

        CreateMap<CreateReviewDto, Review>();
        CreateMap<UpdateReviewDto, Review>();
        CreateMap<Review, ReviewDto>();

        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<Role, RoleDto>();

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        
        CreateMap<User, UserDto>();
        CreateMap<User, UpdateUserDto>();
        
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<CreateOrderStatusDto, OrderStatus>();
        CreateMap<UpdateOrderStatusDto, OrderStatus>();
        
        CreateMap<CreateBlackListedDto, BlackListed>();
        CreateMap<BlackListed, BlackListedDto>().ReverseMap();
        
        CreateMap<CreateUserActiveSessionDto, UserActiveSessions>();
        CreateMap<UpdateUserActiveSessionDto, UserActiveSessions>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UserActiveSessions, UserDto>();
    }
}

using AutoMapper;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateUserCredentialsDto, User>()
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.MapFrom(src => src.RefreshTokenExpiryTime))
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.Ignore())
            .ForMember(dest => dest.LastName, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore());

        CreateMap<UserCredentialsDto, UserDto>();

        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();
        CreateMap<CreateBookDto, BookDto>();

        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));;
        

        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();
        CreateMap<Order, OrderDto>();

        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
        CreateMap<OrderItem, OrderItemDto>();

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

        CreateMap<CreateOrderStatusDto, OrderStatus>();
        CreateMap<UpdateOrderStatusDto, OrderStatus>();
        CreateMap<OrderStatus, OrderStatusDto>();

        CreateMap<UserDto, UserCredentialsDto>();
       
    }
}

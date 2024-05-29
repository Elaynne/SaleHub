using Application.UseCases.Books.CreateBook;
using Application.UseCases.Orders.CreateOrder;
using Application.UseCases.Users.CreateUser;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.MappingProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CreateUserInput, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => true));

            CreateMap<CreateBookInput, Book>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<CreateOrderInput, Order>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.Pending));

            CreateMap<CreateOrderViewModel, CreateOrderInput>();
        }
    }
}

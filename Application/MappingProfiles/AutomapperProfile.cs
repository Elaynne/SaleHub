using Application.UseCases.Users.CreateUser;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CreateUserInput, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}

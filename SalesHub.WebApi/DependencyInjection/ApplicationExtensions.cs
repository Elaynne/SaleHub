using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Domain.Repository.Interfaces;
using Infrastructure.Repository.User;

namespace SalesHub.WebApi.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        { 
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase> ();
            services.AddScoped<IGetUserUseCase, GetUserUseCase>();
            return services;

        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}

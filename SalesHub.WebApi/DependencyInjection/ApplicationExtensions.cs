using Application.UseCases.Auth;
using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
using Domain.Repository.Interfaces;
using Infrastructure.Repository.BookRepository;
using Infrastructure.Repository.User;

namespace SalesHub.WebApi.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        { 
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase> ();
            services.AddScoped<IGetUserUseCase, GetUserUseCase>();
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

            services.AddScoped<ILoginUseCase, LoginUseCase>();
            return services;

        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }
    }
}

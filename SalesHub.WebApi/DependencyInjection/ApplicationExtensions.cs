﻿using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.UpdateUser;
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
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            return services;

        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}

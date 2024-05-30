﻿using Application.UseCases.Books.CreateBook;
using Application.UseCases.Books.DeleteBook;
using Application.UseCases.Books.GetBook;
using Application.UseCases.Books.GetBooks;
using Application.UseCases.Books.UpdateBook;
using Application.UseCases.Orders.CreateOrder;
using Application.UseCases.Orders.GetOrder;
using Application.UseCases.Orders.GetOrders;
using Application.UseCases.Orders.UpdateOrderStatus;
using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.GetUser;
using Application.UseCases.Users.GetUsers;
using Application.UseCases.Users.Login;
using Application.UseCases.Users.UpdateUser;
using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Repository.BookRepository;
using Infrastructure.Repository.OrderRepository;
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

            services.AddScoped<ICreateBookUseCase, CreateBookUseCase>();
            services.AddScoped<IGetBookUseCase, GetBookUseCase>();
            services.AddScoped<IGetBooksUseCase, GetBooksUseCase>();
            services.AddScoped<IUpdateBookUseCase, UpdateBookUseCase>();
            services.AddScoped<IDeleteBookUseCase, DeleteBookUseCase>();

            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IGetOrdersUseCase, GetOrdersUseCase>();
            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
            services.AddScoped<IUpdateOrderStatusUseCase, UpdateOrderStatusUseCase>();
            return services;

        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICacheService<Book>, CacheService<Book>>();
            return services;
        }
    }
}

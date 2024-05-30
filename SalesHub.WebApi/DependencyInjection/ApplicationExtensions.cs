using Application.UseCases.Books.CreateBook;
using Application.UseCases.Books.DeleteBook;
using Application.UseCases.Books.RetrieveBookById;
using Application.UseCases.Books.RetrieveAllBooks;
using Application.UseCases.Books.UpdateBook;
using Application.UseCases.Orders.CreateOrder;
using Application.UseCases.Orders.RetrieveAllOrders;
using Application.UseCases.Orders.RetrieveOrderById;
using Application.UseCases.Orders.CancelOrder;
using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.RetrieveUserById;
using Application.UseCases.Users.RetrieveAllUsers;
using Application.UseCases.Users.Login;
using Application.UseCases.Users.UpdateUser;
using Domain.Cache;
using Domain.Models;
using Domain.Repository.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Repositories.BookRepository;
using Infrastructure.Repositories.OrderRepository;
using Infrastructure.Repositories.User;

namespace SalesHub.WebApi.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        { 
            services.AddScoped<ICreateUser, CreateUser> ();
            services.AddScoped<IRetrieveUserById, RetrieveUserById>();
            services.AddScoped<IRetrieveAllUsers, RetrieveAllUsers>();
            services.AddScoped<IUpdateUser, UpdateUser>();

            services.AddScoped<ILogin, Login>();

            services.AddScoped<ICreateBook, CreateBook>();
            services.AddScoped<IRetrieveBookById, RetrieveBookById>();
            services.AddScoped<IRetrieveAllBooks, RetrieveAllBooks>();
            services.AddScoped<IUpdateBook, UpdateBook>();
            services.AddScoped<IDeleteBook, DeleteBook>();

            services.AddScoped<ICreateOrder, CreateOrder>();
            services.AddScoped<IRetrieveAllOrders, RetrieveAllOrders>();
            services.AddScoped<IRetrieveOrderById, RetrieveOrderById>();
            services.AddScoped<ICanceloOrder, CancelOrder>();
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

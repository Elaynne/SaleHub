using Microsoft.OpenApi.Models;
using SalesHub.WebApi.DependencyInjection;
using Application.UseCases.Users.CreateUser;

namespace SalesHub.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesHub API", Version = "v1" });
            });
            services.AddUseCases();
            services.AddRepositories();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(CreateUserUseCase).Assembly);
            });

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesHub API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

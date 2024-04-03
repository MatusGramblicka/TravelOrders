using Core;
using Interface;
using Microsoft.EntityFrameworkCore;
using Repository;
using TravelOrdersServer.ActionFilters;

namespace TravelOrdersServer.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureTravelOrdersApp(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));

        builder.Services.AddDbContext<TravelOrderDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("TravelOrdersServer").UseNetTopologySuite()));

        builder.Services.AddScoped<ValidationFilterAttribute>();
        builder.Services.AddScoped<ValidateTravelOrderExistsAttribute>();

        builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();
        builder.Services.AddScoped<ICityManager, CityManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
}
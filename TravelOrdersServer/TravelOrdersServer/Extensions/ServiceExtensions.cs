using Core;
using Interface.DatabaseAccess;
using Interface.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
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
                    .WithExposedHeaders("X-Pagination", HeaderNames.ContentDisposition));
        });

    public static void ConfigureTravelOrdersApp(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(Program));

        //builder.Services.AddDbContext<TravelOrderDbContext>(opts =>
        //    opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
        //        b => b.MigrationsAssembly("TravelOrdersServer").UseNetTopologySuite())
        //            //.EnableSensitiveDataLogging()
        //            );

        builder.Services.AddDbContext<TravelOrderDbContext>(opts =>
                opts.UseNpgsql(builder.Configuration.GetConnectionString("postgreSqlConnection"),
                    b => b.MigrationsAssembly("TravelOrdersServer").UseNetTopologySuite())
            //.EnableSensitiveDataLogging()
        );

        builder.Services.AddScoped<ValidationFilterAttribute>();
        builder.Services.AddScoped<ValidateTravelOrderExistsAttribute>();

        builder.Services.AddScoped<ITravelOrderManager, TravelOrderManager>(); 
        builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();
        builder.Services.AddScoped<ICityManager, CityManager>();
        builder.Services.AddScoped<ITrafficManager, TrafficManager>();

        builder.Services.AddScoped<ICsvManager, CsvManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
}
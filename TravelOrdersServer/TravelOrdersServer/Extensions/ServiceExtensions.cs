using Core;
using Interface.DatabaseAccess;
using Interface.Managers;
using Interface.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Repository;
using Repository.Redis;
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

    public static void ConfigureDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        //services.AddDbContext<TravelOrderDbContext>(opts =>
        //    opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
        //        b => b.MigrationsAssembly("TravelOrdersServer").UseNetTopologySuite())
        //            //.EnableSensitiveDataLogging()
        //            );

        services.AddDbContext<TravelOrderDbContext>(opts =>
                opts.UseNpgsql(configuration.GetConnectionString("postgreSqlConnection"),
                    b => b.MigrationsAssembly("TravelOrdersServer")
                        .UseNetTopologySuite()
                        .EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null))
            //.EnableSensitiveDataLogging()
        );
    }

    public static void ConfigureRedis(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "TravelOrdersServer_";
        });

        services.AddScoped<IRedisCacheService, RedisCacheService>();
    }

    public static void ConfigureAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    public static void ConfigureManagers(this IServiceCollection services)
    {
        services.AddScoped<ITravelOrderManager, TravelOrderManager>();
        services.AddScoped<IEmployeeManager, EmployeeManager>();
        services.AddScoped<ICityManager, CityManager>();
        services.AddScoped<ITrafficManager, TrafficManager>();

        services.AddScoped<ICsvManager, CsvManager>();
    }  

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped<ValidateTravelOrderExistsAttribute>();
    }
}
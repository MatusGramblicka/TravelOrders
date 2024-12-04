using Contracts.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TravelOrdersClient.HttpRepository.Interface;
using TravelOrdersClient.HttpRepository;
using Blazored.Toast;
using Syncfusion.Blazor;
using TravelOrdersClient.HttpInterceptor;

namespace TravelOrdersClient.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureTravelOrdersClientApp(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBlazoredToast();

        builder.Services.AddSyncfusionBlazor();

        builder.Services.Configure<ApiConfiguration>
            (builder.Configuration.GetSection("ApiConfiguration"));
        
        builder.Services.AddScoped<HttpInterceptorService>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));
    }

    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITravelOrderHttpRepository, TravelOrderHttpRepository>();
        services.AddScoped<ITrafficHttpRepository, TrafficHttpRepository>();
        services.AddScoped<IEmployeeHttpRepository, EmployeeHttpRepository>();
        services.AddScoped<ICityHttpRepository, CityHttpRepository>();
    }
}
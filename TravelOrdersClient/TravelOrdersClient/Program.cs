using Blazored.Toast;
using Entities.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using TravelOrdersClient;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository;
using TravelOrdersClient.HttpRepository.Interface;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("TravelOrdersAPI", (sp, cl) =>
{
    var apiConfiguration = sp.GetRequiredService<IOptions<ApiConfiguration>>();
    cl.BaseAddress =
        new Uri(apiConfiguration.Value.BaseAddress + "/api/");
    cl.EnableIntercept(sp);
});

builder.Services.AddBlazoredToast();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped(
    sp => sp.GetService<IHttpClientFactory>().CreateClient("TravelOrdersAPI"));

builder.Services.AddHttpClientInterceptor();

builder.Services.AddScoped<HttpInterceptorService>();

builder.Services.AddScoped<ITravelOrderHttpRepository, TravelOrderHttpRepository>();
builder.Services.AddScoped<ITrafficHttpRepository, TrafficHttpRepository>();
builder.Services.AddScoped<IEmployeeHttpRepository, EmployeeHttpRepository>();
builder.Services.AddScoped<ICityHttpRepository, CityHttpRepository>();

builder.Services.Configure<ApiConfiguration>
    (builder.Configuration.GetSection("ApiConfiguration"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

await builder.Build().RunAsync();
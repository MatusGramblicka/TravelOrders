using Contracts.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using TravelOrdersClient;
using TravelOrdersClient.Extensions;

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

builder.Services.AddScoped(
    sp => sp.GetService<IHttpClientFactory>().CreateClient("TravelOrdersAPI"));

builder.ConfigureTravelOrdersClientApp();
builder.Services.ConfigureRepositories();

builder.Services.AddHttpClientInterceptor();

await builder.Build().RunAsync();
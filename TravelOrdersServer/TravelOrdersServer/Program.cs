using Contracts.IntegrationEvents.EventHandlers;
using Contracts.IntegrationEvents.Events;
using Infrastructure.Shared.EventBus;
using Infrastructure.Shared.RabbitMq;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi;
using NLog;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using TravelOrdersServer.Extensions;
using TravelOrdersServer.MigrationManager;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureCors();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAutomapper();
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureFilters();
builder.Services.ConfigureManagers();
builder.Services.ConfigureRepositoryManager();
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqSubscriberService(builder.Configuration)
    .AddEventHandler<TravelOrderCreatedEvent, TravelOrderCreatedEventHandler>();


builder.Services.AddControllers().AddJsonOptions(options => {
    // open api is currently using system.text.json
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new NetTopologySuite.IO.Converters.GeoJsonConverterFactory());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((doc, _, _) =>
    {
        doc.Info = new OpenApiInfo
        {
            Title = "TravelOrders API",
            Version = "v1"
        };
        return Task.CompletedTask;
    });
});

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapOpenApi("/openapi/v1.json");
    app.MapScalarApiReference(); // UI at /scalar
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseDefaultFiles()
    .UseStaticFiles(new StaticFileOptions
    {
        // https://github.com/dotnet/runtime/issues/101992
        // https://stackoverflow.com/questions/75849788/dotnet-application-refusing-to-serve-dll-files-in-wwwroot-needed-by-blazor-weba
        ServeUnknownFileTypes = true
    });

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapFallbackToFile("index.html");
});

app.MigrateDatabase().Run();

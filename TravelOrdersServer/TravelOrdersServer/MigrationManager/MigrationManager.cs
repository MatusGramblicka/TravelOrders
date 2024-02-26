﻿using Microsoft.EntityFrameworkCore;
using Repository;

namespace TravelOrdersServer.MigrationManager;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<TravelOrderDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            appContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Migration was not successful: {ex}");
            throw;
        }

        return host;
    }
}
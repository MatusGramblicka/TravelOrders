using Contracts.RequestFeatures;
using CsvHelper;
using Interface.DatabaseAccess;
using Interface.Managers;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Core;

public class CsvManager(ILogger<CsvManager> logger, IRepositoryManager repository) : ICsvManager
{
    private const int PageSize = 20;

    public async Task GenerateCsv(Stream outputStream)
    {
        try
        {
            logger.LogInformation("Writing vdr csv file");

            await using var vdrStreamWriter = new StreamWriter(outputStream);
            await using var csvWriter = new CsvWriter(vdrStreamWriter, CultureInfo.InvariantCulture);

            var pageNumber = 1;

            var requestParameters = new RequestParameters {PageNumber = pageNumber, PageSize = PageSize};
            var travelOrders = repository.TravelOrder.GetTravelOrdersSelected(requestParameters);
            while (travelOrders.Any())
            {
                logger.LogInformation($"Writing {pageNumber}. batch of csv file");
                await csvWriter.WriteRecordsAsync(travelOrders);
                await csvWriter.FlushAsync();

                pageNumber++;
                requestParameters = new RequestParameters {PageNumber = pageNumber, PageSize = PageSize};
                travelOrders = repository.TravelOrder.GetTravelOrdersSelected(requestParameters);
            }

            logger.LogInformation("Writing vdr csv file finished");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CSV generation failed!");

            throw;
        }
    }
}
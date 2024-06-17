using Contracts.RequestFeatures;
using CsvHelper;
using Interface.DatabaseAccess;
using Interface.Managers;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Core;

public class CsvManager : ICsvManager
{
    private readonly ILogger<CsvManager> _logger;
    private readonly IRepositoryManager _repository;

    private const int PageSize = 20;

    public CsvManager(ILogger<CsvManager> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task GenerateCsv(Stream outputStream)
    {
        try
        {
            _logger.LogInformation("Writing vdr csv file");

            await using var vdrStreamWriter = new StreamWriter(outputStream);
            await using var csvWriter = new CsvWriter(vdrStreamWriter, CultureInfo.InvariantCulture);

            var pageNumber = 1;

            var requestParameters = new RequestParameters {PageNumber = pageNumber, PageSize = PageSize};
            var travelOrders = _repository.TravelOrder.GetAllTravelOrdersSelected(requestParameters);
            while (travelOrders.Any())
            {
                _logger.LogInformation($"Writing {pageNumber}. batch of csv file");
                await csvWriter.WriteRecordsAsync(travelOrders);
                await csvWriter.FlushAsync();

                pageNumber++;
                requestParameters = new RequestParameters {PageNumber = pageNumber, PageSize = PageSize};
                travelOrders = _repository.TravelOrder.GetAllTravelOrdersSelected(requestParameters);
            }

            _logger.LogInformation("Writing vdr csv file finished");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CSV generation failed!");

            throw;
        }
    }
}
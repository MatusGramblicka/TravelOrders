using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class TravelOrderHttpRepository(HttpClient client, IJSRuntime js) : ITravelOrderHttpRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<PagingResponse<TravelOrderSelectedDto>> GetTravelOrders(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm ?? "",
            ["orderBy"] = requestParameters.OrderBy ?? ""
        };

        var response =
            await client.GetAsync(QueryHelpers.AddQueryString("TravelOrder/travelOrdersSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<TravelOrderSelectedDto>
        {
            Items = JsonSerializer.Deserialize<IEnumerable<TravelOrderSelectedDto>>(content, _options) ??
                    new List<TravelOrderSelectedDto>(),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options) ?? new MetaData()
        };

        return pagingResponse;
    }

    public async Task<TravelOrderSelectedDto?> GetTravelOrder(int id)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());

        var travelOrder = await client.GetFromJsonAsync<TravelOrderSelectedDto?>($"travelOrder/travelOrderSelected/{id}", 
            options: options);

        return travelOrder;
    }

    public async Task UpdateTravelOrder(int id, TravelOrderUpdateDto travelOrder)
        => await client.PutAsJsonAsync(Path.Combine("travelOrder",
            id.ToString()), travelOrder);

    public async Task CreateTravelOrder(TravelOrderCreationDto travelOrder)
        => await client.PostAsJsonAsync("travelOrder", travelOrder);

    public async Task DeleteTravelOrder(int id)
        => await client.DeleteAsync(Path.Combine("travelOrder", id.ToString()));
    
    public async Task DownloadCsvFile()
    {
        var fileUrl = $"{client.BaseAddress}csv/file";
        await js.InvokeVoidAsync("triggerFileDownload", fileUrl);
    }
}
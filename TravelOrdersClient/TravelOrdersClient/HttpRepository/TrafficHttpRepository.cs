using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class TrafficHttpRepository(HttpClient client) : ITrafficHttpRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<PagingResponse<TrafficSelectedDto>> GetTraffics(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm ?? "",
            ["orderBy"] = requestParameters.OrderBy ?? ""
        };

        var response =
            await client.GetAsync(QueryHelpers.AddQueryString("Traffic/trafficsSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<TrafficSelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<TrafficSelectedDto>>(content, _options) ??
                    new List<TrafficSelectedDto>(),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options) ?? new MetaData()
        };

        return pagingResponse;
    }
}
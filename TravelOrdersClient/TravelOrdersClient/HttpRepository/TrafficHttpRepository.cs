using Contracts.Dto;
using Entities.RequestFeatures;
using System.Text.Json.Serialization;
using System.Text.Json;
using TravelOrdersClient.Features;
using Microsoft.AspNetCore.WebUtilities;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class TrafficHttpRepository : ITrafficHttpRepository
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public TrafficHttpRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<PagingResponse<TrafficSelectedDto>> GetTraffics(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm == null ? "" : requestParameters.SearchTerm,
            ["orderBy"] = requestParameters.OrderBy == null ? "" : requestParameters.OrderBy
        };

        var response =
            await _client.GetAsync(QueryHelpers.AddQueryString("Traffic/trafficsSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<TrafficSelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<TrafficSelectedDto>>(content, _options),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }
}
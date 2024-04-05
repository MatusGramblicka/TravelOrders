using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class CityHttpRepository : ICityHttpRepository
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public CityHttpRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<PagingResponse<CitySelectedDto>> GetCities(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm == null ? "" : requestParameters.SearchTerm,
            ["orderBy"] = requestParameters.OrderBy == null ? "" : requestParameters.OrderBy
        };

        var response =
            await _client.GetAsync(QueryHelpers.AddQueryString("City/citiesSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<CitySelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<CitySelectedDto>>(content, _options),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }
}
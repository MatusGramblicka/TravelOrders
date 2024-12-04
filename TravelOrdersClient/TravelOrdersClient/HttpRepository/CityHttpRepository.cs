using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class CityHttpRepository(HttpClient client) : ICityHttpRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<PagingResponse<CitySelectedDto>> GetCities(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm ?? "",
            ["orderBy"] = requestParameters.OrderBy ?? ""
        };

        var response =
            await client.GetAsync(QueryHelpers.AddQueryString("City/citiesSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<CitySelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<CitySelectedDto>>(content, _options) ??
                    new List<CitySelectedDto>(),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options) ?? new MetaData()
        };

        return pagingResponse;
    }
}
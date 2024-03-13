using Contracts.Dto;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class EmployeeHttpRepository : IEmployeeHttpRepository
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public EmployeeHttpRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<PagingResponse<EmployeeSelectedDto>> GetEmployees(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm == null ? "" : requestParameters.SearchTerm,
            ["orderBy"] = requestParameters.OrderBy == null ? "" : requestParameters.OrderBy
        };

        var response =
            await _client.GetAsync(QueryHelpers.AddQueryString("Employee/employeesSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<EmployeeSelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<EmployeeSelectedDto>>(content, _options),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }
}
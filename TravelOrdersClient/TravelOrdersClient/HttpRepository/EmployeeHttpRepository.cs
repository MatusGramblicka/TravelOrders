using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelOrdersClient.Features;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.HttpRepository;

public class EmployeeHttpRepository(HttpClient client) : IEmployeeHttpRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<PagingResponse<EmployeeSelectedDto>> GetEmployees(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm ?? "",
            ["orderBy"] = requestParameters.OrderBy ?? ""
        };

        var response =
            await client.GetAsync(QueryHelpers.AddQueryString("Employee/employeesSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<EmployeeSelectedDto>
        {
            Items = JsonSerializer.Deserialize<List<EmployeeSelectedDto>>(content, _options) ??
                    new List<EmployeeSelectedDto>(),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options) ?? new MetaData()
        };

        return pagingResponse;
    }
}
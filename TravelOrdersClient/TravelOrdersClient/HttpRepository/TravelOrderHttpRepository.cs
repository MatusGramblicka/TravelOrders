﻿using Blazored.Toast.Services;
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

public class TravelOrderHttpRepository : ITravelOrderHttpRepository
{
    private readonly HttpClient _client;
    private readonly IJSRuntime _js;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public TravelOrderHttpRepository(HttpClient client, IJSRuntime js)
    {
        _client = client;
        _js = js;
    }

    public async Task<PagingResponse<TravelOrderSelectedDto>> GetTravelOrders(RequestParameters requestParameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = requestParameters.PageNumber.ToString(),
            ["pageSize"] = requestParameters.PageSize.ToString(),
            ["searchTerm"] = requestParameters.SearchTerm == null ? "" : requestParameters.SearchTerm,
            ["orderBy"] = requestParameters.OrderBy == null ? "" : requestParameters.OrderBy
        };

        var response =
            await _client.GetAsync(QueryHelpers.AddQueryString("TravelOrder/travelOrdersSelected", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<TravelOrderSelectedDto>
        {
            Items = JsonSerializer.Deserialize<IEnumerable<TravelOrderSelectedDto>>(content, _options),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }

    public async Task<TravelOrderSelectedDto?> GetTravelOrder(int id)
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());

        var travelOrder = await _client.GetFromJsonAsync<TravelOrderSelectedDto?>($"travelOrder/travelOrderSelected/{id}", 
            options: options);

        return travelOrder;
    }

    public async Task UpdateTravelOrder(int id, TravelOrderUpdateDto travelOrder)
        => await _client.PutAsJsonAsync(Path.Combine("travelOrder",
            id.ToString()), travelOrder);

    public async Task CreateTravelOrder(TravelOrderCreationDto travelOrder)
        => await _client.PostAsJsonAsync("travelOrder", travelOrder);

    public async Task DeleteTravelOrder(int id)
        => await _client.DeleteAsync(Path.Combine("travelOrder", id.ToString()));
    
    public async Task DownloadCsvFile()
    {
        var fileUrl = $"{_client.BaseAddress}csv/file";
        await _js.InvokeVoidAsync("triggerFileDownload", fileUrl);
    }
}
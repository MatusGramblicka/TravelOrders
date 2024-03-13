using Contracts.Dto;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages.Deprecated;

public partial class TravelOrderPage : IDisposable
{
    public List<TravelOrderSelectedDto> TravelOrderList { get; set; } = new();
    public MetaData MetaData { get; set; } = new();

    public RequestParameters RequestParameters = new();

    [Inject]
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject]
    public HttpInterceptorService Interceptor { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Interceptor.RegisterEvent();
        await GetTravelOrders();
    }

    private async Task SelectedPage(int page)
    {
        RequestParameters.PageNumber = page;
        await GetTravelOrders();
    }

    private async Task GetTravelOrders()
    {
        var pagingResponse = await TravelOrderRepo.GetTravelOrders(RequestParameters);

        TravelOrderList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    private async Task SetPageSize(int pageSize)
    {
        RequestParameters.PageSize = pageSize;
        RequestParameters.PageNumber = 1;

        await GetTravelOrders();
    }

    private async Task SearchChanged(string searchTerm)
    {
        RequestParameters.PageNumber = 1;
        RequestParameters.SearchTerm = searchTerm;

        await GetTravelOrders();
    }

    private async Task DeleteTravelOrder(int id)
    {
        await TravelOrderRepo.DeleteTravelOrder(id);

        if (RequestParameters.PageNumber > 1 && TravelOrderList.Count == 1)
            RequestParameters.PageNumber--;

        await GetTravelOrders();
    }

    public void Dispose() => Interceptor.DisposeEvent();
}
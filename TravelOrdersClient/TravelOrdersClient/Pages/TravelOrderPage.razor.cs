using Contracts.Dto;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository;

namespace TravelOrdersClient.Pages;

public partial class TravelOrderPage : IDisposable
{
    public List<TravelOrderSelectedDto> TravelOrderList { get; set; } = new List<TravelOrderSelectedDto>();
    public MetaData MetaData { get; set; } = new MetaData();

    public RequestParameters _requestParameters = new RequestParameters();

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
        _requestParameters.PageNumber = page;
        await GetTravelOrders();
    }

    private async Task GetTravelOrders()
    {
        var pagingResponse = await TravelOrderRepo.GetTravelOrders(_requestParameters);

        TravelOrderList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    private async Task SetPageSize(int pageSize)
    {
        _requestParameters.PageSize = pageSize;
        _requestParameters.PageNumber = 1;

        await GetTravelOrders();
    }

    private async Task SearchChanged(string searchTerm)
    {
        _requestParameters.PageNumber = 1;
        _requestParameters.SearchTerm = searchTerm;

        await GetTravelOrders();
    }

    private async Task DeleteTravelOrder(int id)
    {
        await TravelOrderRepo.DeleteTravelOrder(id);

        if (_requestParameters.PageNumber > 1 && TravelOrderList.Count == 1)
            _requestParameters.PageNumber--;

        await GetTravelOrders();
    }

    public void Dispose() => Interceptor.DisposeEvent();

}
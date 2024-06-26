﻿using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.Components;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages;

public partial class TravelOrderPage : IDisposable
{
    public IEnumerable<TravelOrderSelectedDto> TravelOrderList { get; set; } = null!;

    public MetaData MetaData { get; set; } = new();

    public RequestParameters RequestParameters = new();

    [Inject]
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject]
    public HttpInterceptorService Interceptor { get; set; }
    
    private bool _alreadyDisposed;

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
        
        if (RequestParameters.PageNumber > 1 && TravelOrderList.Count() == 1)
            RequestParameters.PageNumber--;

        await GetTravelOrders();
    }

    private async Task DownloadFile()
    {
        await TravelOrderRepo.DownloadCsvFile();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_alreadyDisposed)
            return;

        if (disposing)
        {
            Interceptor.DisposeEvent();
          
            _alreadyDisposed = true;
        }
    }

    ~TravelOrderPage()
    {
        Dispose(disposing: false);
    }
}
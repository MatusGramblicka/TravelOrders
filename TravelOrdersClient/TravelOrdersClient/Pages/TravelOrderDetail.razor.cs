using Contracts.Dto;
using Microsoft.AspNetCore.Components;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages;

public partial class TravelOrderDetail
{
    private TravelOrderSelectedDto TravelOrderSelectedDto { get; set; } = new();

    [Inject]
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Parameter]
    public int TravelOrderId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var travelOrderFromRepo = await TravelOrderRepo.GetTravelOrder(TravelOrderId);
        TravelOrderSelectedDto = travelOrderFromRepo ?? new TravelOrderSelectedDto();
    }
}
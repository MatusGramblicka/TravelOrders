using Contracts.Dto;
using Microsoft.AspNetCore.Components;
using TravelOrdersClient.Shared;

namespace TravelOrdersClient.Components;

public partial class TravelOrderTable
{
    [Parameter]
    public List<TravelOrderSelectedDto> TravelOrders { get; set; } = new();

    [Parameter]
    public EventCallback<int> OnDelete { get; set; }

    private Confirmation _confirmation;

    private int _travelIdOrderToDelete;

    private void CallConfirmationModal(int id)
    {
        _travelIdOrderToDelete = id;
        _confirmation.Show();
    }

    private async Task DeleteTravelOrder()
    {
        _confirmation.Hide();
        await OnDelete.InvokeAsync(_travelIdOrderToDelete);
    }
}
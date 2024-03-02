using Microsoft.AspNetCore.Components;

namespace TravelOrdersClient.Components;

public partial class PageSizeDropDown
{
    [Parameter]
    public EventCallback<int> SelectedPageSize { get; set; }

    private async Task OnPageSizeChange(ChangeEventArgs eventArgs)
    {
        await SelectedPageSize.InvokeAsync(int.Parse(eventArgs.Value.ToString()));
    }
}
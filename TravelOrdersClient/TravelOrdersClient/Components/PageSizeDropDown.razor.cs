using Microsoft.AspNetCore.Components;

namespace TravelOrdersClient.Components;

public partial class PageSizeDropDown
{
    [Parameter]
    public EventCallback<int> SelectedPageSize { get; set; }

    private async Task OnPageSizeChange(ChangeEventArgs eventArgs)
    {
        var converted = int.TryParse(eventArgs.Value?.ToString(), out var result);

        if (converted)
            await SelectedPageSize.InvokeAsync(result);
    }
}
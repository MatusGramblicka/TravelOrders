using Blazored.Toast.Services;
using Contracts.Dto;
using Contracts.Enums;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository;

namespace TravelOrdersClient.Pages;

public partial class CreateTravelOrder
{
    private TravelOrderCreationDto _travelOrder = new();
    public List<TrafficSelectedDto> TrafficList { get; set; } = new();

    public RequestParameters _requestParameters = new();

    private EditContext _editContext;
    private bool _formInvalid = true;

    [Inject]
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject]
    public ITrafficHttpRepository TrafficRepo { get; set; }

    [Inject]
    public HttpInterceptorService Interceptor { get; set; }

    [Inject]
    public IToastService ToastService { get; set; }
    
    private List<TrafficSelectedDto?> _selectedTraffics;

    protected override async Task OnInitializedAsync()
    {
        _travelOrder.StartDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
        _travelOrder.EndDate = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
        _editContext = new EditContext(_travelOrder);
        _editContext.OnFieldChanged += HandleFieldChanged;
        Interceptor.RegisterEvent();

        var pagingResponse = await TrafficRepo.GetTraffics(_requestParameters);
        TrafficList = pagingResponse.Items;
    }

    private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
    {
        _formInvalid = !_editContext.Validate();
        StateHasChanged();
    }

    private async Task Create()
    {
        var trafficsToAdd = new List<TrafficSelectedDto>();
        foreach (var selectedTrafficDd in _selectedTraffics.Select(s => s!.Id))
        {
            var selectedTraffic = TrafficList.SingleOrDefault(t => t.Id == selectedTrafficDd);
            if (selectedTraffic != null)
            {
                trafficsToAdd.Add(selectedTraffic);
            }
        }

        _travelOrder.Traffics = trafficsToAdd;

        await TravelOrderRepo.CreateTravelOrder(_travelOrder);

        ToastService.ShowSuccess($"Action successful: travel order successfully added.");
        _travelOrder = new TravelOrderCreationDto();
        _editContext.OnValidationStateChanged += ValidationChanged;
        _editContext.NotifyValidationStateChanged();
    }

    private void ValidationChanged(object sender, ValidationStateChangedEventArgs e)
    {
        _formInvalid = true;
        _editContext.OnFieldChanged -= HandleFieldChanged;
        _editContext = new EditContext(_travelOrder);
        _editContext.OnFieldChanged += HandleFieldChanged;
        _editContext.OnValidationStateChanged -= ValidationChanged;
    }
        
    public void Dispose()
    {
        Interceptor.DisposeEvent();
        _editContext.OnFieldChanged -= HandleFieldChanged;
        _editContext.OnValidationStateChanged -= ValidationChanged;
    }
}
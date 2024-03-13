using Blazored.Toast.Services;
using Contracts.Dto;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages.Improved;

public partial class CreateTravelOrderImproved
{
    private TravelOrderCreationDto _travelOrder = new();
    public List<TrafficSelectedDto> TrafficList { get; set; } = new();
    public List<EmployeeSelectedDto> EmployeeList { get; set; } = new();
    public List<CitySelectedDto> CityList { get; set; } = new();

    public RequestParameters RequestParameters = new();

    private EditContext _editContext;
    private bool _formInvalid = true;

    [Inject]
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject]
    public ITrafficHttpRepository TrafficRepo { get; set; }

    [Inject]
    public IEmployeeHttpRepository EmployeeRepo { get; set; }

    [Inject]
    public ICityHttpRepository CityRepo { get; set; }

    [Inject]
    public HttpInterceptorService Interceptor { get; set; }

    [Inject]
    public IToastService ToastService { get; set; }

    private List<TrafficSelectedDto?> _selectedTraffics = new();

    protected override async Task OnInitializedAsync()
    {
        _travelOrder.StartDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
        _travelOrder.EndDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
        _editContext = new EditContext(_travelOrder);
        _editContext.OnFieldChanged += HandleFieldChanged;
        Interceptor.RegisterEvent();

        var pagingResponseTraffics = await TrafficRepo.GetTraffics(RequestParameters);
        TrafficList = pagingResponseTraffics.Items;

        var pagingResponseEmployees = await EmployeeRepo.GetEmployees(RequestParameters);
        EmployeeList = pagingResponseEmployees.Items;
        
        var pagingResponseCities = await CityRepo.GetCities(RequestParameters);
        CityList = pagingResponseCities.Items;
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
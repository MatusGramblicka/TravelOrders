using AutoMapper;
using Blazored.Toast.Services;
using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages;

public partial class UpdateTravelOrder : IDisposable
{
    private TravelOrderSelectedDto? _travelOrder;

    private List<TrafficSelectedDto?> _selectedTraffics = new();

    private TravelOrderUpdateDto TravelOrderUpdateDto { get; set; } = null!;

    public List<EmployeeSelectedDto> EmployeeList { get; set; } = new();

    public List<CitySelectedDto> CityList { get; set; } = new();

    public List<TrafficSelectedDto> TrafficList { get; set; } = new();

    public RequestParameters RequestParameters = new();

    private EditContext _editContext = null!;
    private bool _formInvalid = true;

    private bool _alreadyDisposed;

    [Inject] 
    public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject] 
    public IEmployeeHttpRepository EmployeeRepo { get; set; }

    [Inject] 
    public ICityHttpRepository CityRepo { get; set; }


    [Inject] 
    public ITrafficHttpRepository TrafficRepo { get; set; }

    [Inject] 
    public HttpInterceptorService Interceptor { get; set; }

    [Inject] 
    public IToastService ToastService { get; set; }

    [Inject] 
    public IMapper Mapper { get; set; }

    [Parameter] 
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var pagingResponseEmployees = await EmployeeRepo.GetEmployees(RequestParameters);
        EmployeeList = pagingResponseEmployees.Items;

        var pagingResponseCities = await CityRepo.GetCities(RequestParameters);
        CityList = pagingResponseCities.Items;

        _travelOrder = await TravelOrderRepo.GetTravelOrder(Id);

        if (_travelOrder != null)
        {
            TravelOrderUpdateDto = new TravelOrderUpdateDto
            {
                EmployeeId = _travelOrder.Tenant.Id,
                EndPlaceCityId = _travelOrder.EndPlace.Id,
                StartPlaceCityId = _travelOrder.StartPlace.Id,
                EndDate = _travelOrder.EndDate,
                StartDate = _travelOrder.StartDate,
                State = _travelOrder.State,
                Traffics = _travelOrder.Traffics,
                Note = _travelOrder.Note
            };
        }

        var pagingResponse = await TrafficRepo.GetTraffics(RequestParameters);
        TrafficList = pagingResponse.Items;
        _selectedTraffics = TravelOrderUpdateDto.Traffics.ToList();

        _editContext = new EditContext(TravelOrderUpdateDto);
        _editContext.OnFieldChanged += HandleFieldChanged;
        Interceptor.RegisterEvent();
    }

    private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
    { 
        _formInvalid = !_editContext.Validate();
        StateHasChanged();
    }

    private async Task Update()
    {
        var trafficsToAdd = new List<TrafficSelectedDto>();

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract,
        // UI component can set it to null
        if (_selectedTraffics != null)
        {
            foreach (var selectedTrafficDd in _selectedTraffics.Select(s => s?.Id))
            {
                var selectedTraffic = TrafficList.SingleOrDefault(t => t.Id == selectedTrafficDd);

                if (selectedTraffic != null)
                    trafficsToAdd.Add(selectedTraffic);
            }
        }

        TravelOrderUpdateDto.Traffics = trafficsToAdd;

        if (_travelOrder != null)
        {
            await TravelOrderRepo.UpdateTravelOrder(_travelOrder.Id, TravelOrderUpdateDto);
            ToastService.ShowSuccess("Action successful: Travel order was successfully updated.");
        }
        else
            ToastService.ShowError("Action unsuccessful: Travel order was not updated.");
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
            _editContext.OnFieldChanged -= HandleFieldChanged;

            _alreadyDisposed = true;
        }
    }

    ~UpdateTravelOrder()
    {
        Dispose(disposing: false);
    }
}
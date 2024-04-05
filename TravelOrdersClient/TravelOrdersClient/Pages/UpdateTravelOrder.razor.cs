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
    private TravelOrderSelectedDto _travelOrder;

    private List<TrafficSelectedDto?> _selectedTraffics = new();

    private TravelOrderUpdateDto TravelOrderUpdateDto { get; set; }

    public List<EmployeeSelectedDto> EmployeeList { get; set; } = new();

    public List<CitySelectedDto> CityList { get; set; } = new();

    public List<TrafficSelectedDto> TrafficList { get; set; } = new();

    public RequestParameters RequestParameters = new();

    private EditContext _editContext;
    private bool formInvalid = true;

    [Inject] public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject] public IEmployeeHttpRepository EmployeeRepo { get; set; }

    [Inject] public ICityHttpRepository CityRepo { get; set; }


    [Inject] public ITrafficHttpRepository TrafficRepo { get; set; }

    [Inject] public HttpInterceptorService Interceptor { get; set; }

    [Inject] public IToastService ToastService { get; set; }

    [Inject] public IMapper Mapper { get; set; }

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var pagingResponseEmployees = await EmployeeRepo.GetEmployees(RequestParameters);
        EmployeeList = pagingResponseEmployees.Items;

        var pagingResponseCities = await CityRepo.GetCities(RequestParameters);
        CityList = pagingResponseCities.Items;

        _travelOrder = await TravelOrderRepo.GetTravelOrder(Id);
        TravelOrderUpdateDto = new TravelOrderUpdateDto
        {
            EmployeeId = _travelOrder.Tenant.Id,
            EndPlaceCityId = _travelOrder.EndPlace.Id,
            StartPlaceCityId = _travelOrder.StartPlace.Id,
            EndDate = _travelOrder.EndDate,
            StartDate = _travelOrder.StartDate,
            State = _travelOrder.State,
            Traffics = _travelOrder.Traffics
        };

        var pagingResponse = await TrafficRepo.GetTraffics(RequestParameters);
        TrafficList = pagingResponse.Items;
        _selectedTraffics = TravelOrderUpdateDto.Traffics.ToList();

        _editContext = new EditContext(TravelOrderUpdateDto);
        _editContext.OnFieldChanged += HandleFieldChanged;
        Interceptor.RegisterEvent();
    }

    private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
    {
        formInvalid = !_editContext.Validate();
        StateHasChanged();
    }

    private async Task Update()
    {
        var trafficsToAdd = new List<TrafficSelectedDto>();
        if (_selectedTraffics != null)
        {
            foreach (var selectedTrafficDd in _selectedTraffics.Select(s => s!.Id))
            {
                var selectedTraffic = TrafficList.SingleOrDefault(t => t.Id == selectedTrafficDd);
                if (selectedTraffic != null)
                {
                    trafficsToAdd.Add(selectedTraffic);
                }
            }
        }

        TravelOrderUpdateDto.Traffics = trafficsToAdd;

        await TravelOrderRepo.UpdateTravelOrder(_travelOrder.Id, TravelOrderUpdateDto);

        ToastService.ShowSuccess($"Action successful: TravelOrderUpdateDto successfully updated.");
    }

    public void Dispose()
    {
        Interceptor.DisposeEvent();
        _editContext.OnFieldChanged -= HandleFieldChanged;
    }
}
using Blazored.Toast.Services;
using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages.Improved;

public partial class CreateTravelOrderImproved
{
    private TravelOrderCreationDto _travelOrder = new();
    public List<TrafficSelectedDto> TrafficList { get; set; } = new();
    public List<EmployeeSelectedDto> EmployeeList { get; set; } = new();
    public List<CitySelectedDto> CityList { get; set; } = new();
    public List<CitySelectedDto> StartPlaceCityList { get; set; } = new();
    public List<CitySelectedDto> EndPlaceCityList { get; set; } = new();

    public RequestParameters RequestParameters = new();

    private EditContext _editContext;
    private bool _formInvalid = true;

    private int _countStartPlaceCity;
    private int? _pageNumberStartPlaceCity = 1;

    private int _countEndPlaceCity;
    private int? _pageNumberEndPlaceCity = 1;

    [Inject] public ITravelOrderHttpRepository TravelOrderRepo { get; set; }

    [Inject] public ITrafficHttpRepository TrafficRepo { get; set; }

    [Inject] public IEmployeeHttpRepository EmployeeRepo { get; set; }

    [Inject] public ICityHttpRepository CityRepo { get; set; }

    [Inject] public HttpInterceptorService Interceptor { get; set; }

    [Inject] public IToastService ToastService { get; set; }

    private List<TrafficSelectedDto?> _selectedTraffics = new();

    protected override async Task OnInitializedAsync()
    {
        _travelOrder.StartDate = DateTime.Today;
        _travelOrder.EndDate = DateTime.Today;

        _editContext = new EditContext(_travelOrder);
        _editContext.OnFieldChanged += HandleFieldChanged;

        Interceptor.RegisterEvent();

        var pagingResponseTraffics = await TrafficRepo.GetTraffics(RequestParameters);
        TrafficList = pagingResponseTraffics.Items;

        var pagingResponseEmployees = await EmployeeRepo.GetEmployees(RequestParameters);
        EmployeeList = pagingResponseEmployees.Items;

        var pagingResponseCities = await CityRepo.GetCities(RequestParameters);
        CityList = pagingResponseCities.Items;

        await LoadDataStartPlaceCities(new LoadDataArgs {Top = 5, Skip = 0});
        await LoadDataEndPlaceCities(new LoadDataArgs { Top = 5, Skip = 0 });
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

    private async Task LoadDataStartPlaceCities(LoadDataArgs args)
    {
        if (args.Skip != null || args.Top != null)
        {
            _pageNumberStartPlaceCity = args.Skip / args.Top + 1;
        }

        var requestParameters = new RequestParameters
        {
            PageSize = args.Top ?? 5,
            PageNumber = _pageNumberStartPlaceCity ?? 1
        };

        var pagingResponseCities = await CityRepo.GetCities(requestParameters);
        StartPlaceCityList = pagingResponseCities.Items;

        _countStartPlaceCity = pagingResponseCities.MetaData.TotalCount;

        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadDataEndPlaceCities(LoadDataArgs args)
    {
        if (args.Skip != null || args.Top != null)
        {
            _pageNumberEndPlaceCity = args.Skip / args.Top + 1;
        }

        var requestParameters = new RequestParameters
        {
            PageSize = args.Top ?? 5,
            PageNumber = _pageNumberEndPlaceCity ?? 1
        };

        var pagingResponseCities = await CityRepo.GetCities(requestParameters);
        EndPlaceCityList = pagingResponseCities.Items;

        _countEndPlaceCity = pagingResponseCities.MetaData.TotalCount;

        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Interceptor.DisposeEvent();
        _editContext.OnFieldChanged -= HandleFieldChanged;
        _editContext.OnValidationStateChanged -= ValidationChanged;
    }
}
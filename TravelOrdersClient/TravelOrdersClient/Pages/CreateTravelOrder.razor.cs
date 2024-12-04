using Blazored.Toast.Services;
using Contracts.Dto;
using Contracts.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using TravelOrdersClient.HttpInterceptor;
using TravelOrdersClient.HttpRepository.Interface;

namespace TravelOrdersClient.Pages;

public partial class CreateTravelOrder : IDisposable
{
    private TravelOrderCreationDto _travelOrderCreationDto = new();

    public IEnumerable<TrafficSelectedDto> TrafficList { get; set; } = null!;

    public IEnumerable<EmployeeSelectedDto> EmployeeList { get; set; } = null!;

    public IEnumerable<CitySelectedDto> CityList { get; set; } = null!;

    public IEnumerable<CitySelectedDto> StartPlaceCityList { get; set; } = null!;

    public IEnumerable<CitySelectedDto> EndPlaceCityList { get; set; } = null!;

    private List<TrafficSelectedDto?> _selectedTraffics = new();

    public RequestParameters RequestParameters = new();

    private EditContext _editContext = null!;
    private bool _formInvalid = true;

    private int _countStartPlaceCity;
    private int? _pageNumberStartPlaceCity = 1;

    private int _countEndPlaceCity;
    private int? _pageNumberEndPlaceCity = 1;

    private bool _alreadyDisposed;

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
    
    protected override async Task OnInitializedAsync()
    {
        _travelOrderCreationDto.StartDate = DateTime.Today;
        _travelOrderCreationDto.EndDate = DateTime.Today;

        _editContext = new EditContext(_travelOrderCreationDto);
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
        foreach (var selectedTrafficDd in _selectedTraffics.Select(s => s?.Id))
        {
            var selectedTraffic = TrafficList.SingleOrDefault(t => t.Id == selectedTrafficDd);
            if (selectedTraffic != null)
                trafficsToAdd.Add(selectedTraffic);
        }

        _travelOrderCreationDto.Traffics = trafficsToAdd;

        await TravelOrderRepo.CreateTravelOrder(_travelOrderCreationDto);

        ToastService.ShowSuccess("Action successful: Travel order was successfully added.");

        _travelOrderCreationDto = new TravelOrderCreationDto
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today,
        };
        _selectedTraffics= new();

        _editContext.OnValidationStateChanged += ValidationChanged;
        _editContext.NotifyValidationStateChanged();
    }

    private void ValidationChanged(object sender, ValidationStateChangedEventArgs e)
    {
        _formInvalid = true;

        _editContext.OnFieldChanged -= HandleFieldChanged;
        _editContext = new EditContext(_travelOrderCreationDto);
        _editContext.OnFieldChanged += HandleFieldChanged;
        _editContext.OnValidationStateChanged -= ValidationChanged;
    }

    private async Task LoadDataStartPlaceCities(LoadDataArgs args)
    {
        if (args.Skip != null || args.Top != null)
            _pageNumberStartPlaceCity = args.Skip / args.Top + 1;

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
            _pageNumberEndPlaceCity = args.Skip / args.Top + 1;

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
            _editContext.OnValidationStateChanged -= ValidationChanged;

            _alreadyDisposed = true;
        }
    }

    ~CreateTravelOrder()
    {
        Dispose(disposing: false);
    }
}
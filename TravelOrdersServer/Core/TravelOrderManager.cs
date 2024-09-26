using AutoMapper;
using Contracts.Dto;
using Contracts.Exceptions;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class TravelOrderManager : ITravelOrderManager
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public TravelOrderManager(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedList<TravelOrderSelectedDto> GetAllTravelOrdersSelected(RequestParameters requestParameters)
    {
        if (requestParameters == null)
            throw new ArgumentNullException(nameof(requestParameters));

        return _repository.TravelOrder.GetAllTravelOrdersSelected(requestParameters);
    }

    public async Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int id)
    {
        return await _repository.TravelOrder.GetTravelOrderSelectedAsync(id);
    }

    public async Task<TravelOrderDto> CreateTravelOrderAsync(TravelOrderCreationDto travelOrderDto)
    {
        if (travelOrderDto == null)
            throw new ArgumentNullException(nameof(travelOrderDto));

        var startCity = await _repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity == null)
            throw new CityMissingException("The city record does not exist");

        var endCity = await _repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity == null)
            throw new CityMissingException("The city record does not exist");

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee == null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrderDto.Traffics.Count())
            throw new TrafficMissingException("The traffic record does not exist");

        var travelOrderEntity = _mapper.Map<TravelOrder>(travelOrderDto);
        travelOrderEntity.Traffics = (ICollection<Traffic>) traffics;

        _repository.TravelOrder.CreateTravelOrder(travelOrderEntity);
        await _repository.SaveAsync();

        return _mapper.Map<TravelOrderDto>(travelOrderEntity);
    }

    public async Task UpdateTravelOrder(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto)
    {
        if (travelOrderFromDb == null)
            throw new ArgumentNullException(nameof(travelOrderFromDb));
        if (travelOrderDto == null)
            throw new ArgumentNullException(nameof(travelOrderDto));

        var startCity = await _repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity == null)
            throw new CityMissingException("The city record does not exist");

        var endCity = await _repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity == null)
            throw new CityMissingException("The city record does not exist");

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee == null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrderDto.Traffics.Count())
            throw new TrafficMissingException("The traffic record does not exist");

        _mapper.Map(travelOrderDto, travelOrderFromDb);
        travelOrderFromDb.Traffics.Clear();
        travelOrderFromDb.Traffics = (ICollection<Traffic>) traffics;

        await _repository.SaveAsync();
    }

    public async Task UpdateTravelOrderDirectMapping(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto)
    {
        if (travelOrderFromDb == null)
            throw new ArgumentNullException(nameof(travelOrderFromDb));
        if (travelOrderDto == null)
            throw new ArgumentNullException(nameof(travelOrderDto));

        var startCity = await _repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity == null)
            throw new CityMissingException("The city record does not exist");
        
        var endCity = await _repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity == null)
            throw new CityMissingException("The city record does not exist");

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee == null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), false);
        if (traffics.Count() != travelOrderDto.Traffics.Count())
            throw new TrafficMissingException("The traffic record does not exist");

        var existingTrafficsIds = travelOrderFromDb.Traffics.Select(x => x.Id).ToList();
        var updatedTrafficsIds = travelOrderDto.Traffics.Select(x => x.Id).ToList();
        var trafficIdsToAdd = updatedTrafficsIds.Except(existingTrafficsIds).ToList();
        var trafficIdsToRemove = existingTrafficsIds.Except(updatedTrafficsIds).ToList();
        
        // https://stackoverflow.com/questions/76359992/what-is-the-proper-way-to-update-delete-many-to-many-relationships-in-entity-fra
        travelOrderFromDb.EmployeeId = travelOrderDto.EmployeeId;
        travelOrderFromDb.StartPlaceCityId = travelOrderDto.StartPlaceCityId;
        travelOrderFromDb.EndPlaceCityId = travelOrderDto.EndPlaceCityId;
        travelOrderFromDb.StartDate = travelOrderDto.StartDate;
        travelOrderFromDb.EndDate = travelOrderDto.EndDate;
        travelOrderFromDb.Note = travelOrderDto.Note;
        travelOrderFromDb.State = travelOrderDto.State;
        
        if (trafficIdsToRemove.Count != 0)
        {
            var trafficsToRemove = travelOrderFromDb.Traffics.Where(x => trafficIdsToRemove.Contains(x.Id)).ToList();
            foreach (var traffic in trafficsToRemove)
                travelOrderFromDb.Traffics.Remove(traffic);
        }

        if (trafficIdsToAdd.Count != 0)
        {
            var trafficsToAdd = await _repository.Traffic.GetByIdsAsync(trafficIdsToAdd, false);
            foreach (var traffic in trafficsToAdd)
                travelOrderFromDb.Traffics.Add(traffic);
        }

        await _repository.SaveAsync();
    }

    public async Task DeleteTravelOrder(TravelOrder travelOrderDb)
    {
        if (travelOrderDb == null)
            throw new ArgumentNullException(nameof(travelOrderDb));

        _repository.TravelOrder.DeleteTravelOrder(travelOrderDb);
        await _repository.SaveAsync();
    }
}
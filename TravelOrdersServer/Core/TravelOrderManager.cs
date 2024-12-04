using AutoMapper;
using Contracts.Dto;
using Contracts.Exceptions;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class TravelOrderManager(IRepositoryManager repository, IMapper mapper) : ITravelOrderManager
{
    public PagedList<TravelOrderSelectedDto> GetTravelOrdersSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        return repository.TravelOrder.GetTravelOrdersSelected(requestParameters);
    }

    public async Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int id)
    {
        return await repository.TravelOrder.GetTravelOrderSelectedAsync(id);
    }

    public async Task<TravelOrderDto> CreateTravelOrderAsync(TravelOrderCreationDto travelOrderDto)
    {
        ArgumentNullException.ThrowIfNull(travelOrderDto, nameof(travelOrderDto));

        var startCity = await repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity is null)
            throw new CityMissingException("The city record does not exist");

        var endCity = await repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity is null)
            throw new CityMissingException("The city record does not exist");

        var employee = await repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee is null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrderDto.Traffics.Count())
            throw new TrafficMissingException("The traffic record does not exist");

        var travelOrderEntity = mapper.Map<TravelOrder>(travelOrderDto);
        travelOrderEntity.Traffics = (ICollection<Traffic>) traffics;

        repository.TravelOrder.CreateTravelOrder(travelOrderEntity);
        await repository.SaveAsync();

        return mapper.Map<TravelOrderDto>(travelOrderEntity);
    }

    public async Task UpdateTravelOrder(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto)
    {
        ArgumentNullException.ThrowIfNull(travelOrderFromDb, nameof(travelOrderFromDb));
        ArgumentNullException.ThrowIfNull(travelOrderDto, nameof(travelOrderDto));

        var startCity = await repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity is null)
            throw new CityMissingException("The city record does not exist");

        var endCity = await repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity is null)
            throw new CityMissingException("The city record does not exist");

        var employee = await repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee is null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrderDto.Traffics.Count())
            throw new TrafficMissingException("The traffic record does not exist");

        mapper.Map(travelOrderDto, travelOrderFromDb);
        travelOrderFromDb.Traffics.Clear();
        travelOrderFromDb.Traffics = (ICollection<Traffic>) traffics;

        await repository.SaveAsync();
    }

    public async Task UpdateTravelOrderDirectMapping(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto)
    {
        ArgumentNullException.ThrowIfNull(travelOrderFromDb, nameof(travelOrderFromDb));
        ArgumentNullException.ThrowIfNull(travelOrderDto, nameof(travelOrderDto));

        var startCity = await repository.City.GetCityAsync(travelOrderDto.StartPlaceCityId, false);
        if (startCity is null)
            throw new CityMissingException("The city record does not exist");
        
        var endCity = await repository.City.GetCityAsync(travelOrderDto.EndPlaceCityId, false);
        if (endCity is null)
            throw new CityMissingException("The city record does not exist");

        var employee = await repository.Employee.GetEmployeeAsync(travelOrderDto.EmployeeId, false);
        if (employee is null)
            throw new EmployeeMissingException("The employee record does not exist");

        var traffics = await repository.Traffic.GetByIdsAsync(travelOrderDto.Traffics.Select(t => t.Id), false);
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
            var trafficsToAdd = await repository.Traffic.GetByIdsAsync(trafficIdsToAdd, false);
            foreach (var traffic in trafficsToAdd)
                travelOrderFromDb.Traffics.Add(traffic);
        }

        await repository.SaveAsync();
    }

    public async Task DeleteTravelOrder(TravelOrder travelOrderDb)
    {
        ArgumentNullException.ThrowIfNull(travelOrderDb, nameof(travelOrderDb));

        repository.TravelOrder.DeleteTravelOrder(travelOrderDb);
        await repository.SaveAsync();
    }
}
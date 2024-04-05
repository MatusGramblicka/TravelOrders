using AutoMapper;
using Contracts.Dto;
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
        return _repository.TravelOrder.GetAllTravelOrdersSelected(requestParameters);
    }

    public async Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int id)
    {
        return await _repository.TravelOrder.GetTravelOrderSelectedAsync(id);
    }

    public async Task<TravelOrderDto> CreateTravelOrderAsync(TravelOrderCreationDto travelOrder)
    {
        var startCity = await _repository.City.GetCityAsync(travelOrder.StartPlaceCityId, false);
        if (startCity == null)
            throw new Exception();

        var endCity = await _repository.City.GetCityAsync(travelOrder.EndPlaceCityId, false);
        if (endCity == null)
            throw new Exception();

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrder.EmployeeId, false);
        if (employee == null)
            throw new Exception();

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrder.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrder.Traffics.Count)
            throw new Exception();

        var travelOrderEntity = _mapper.Map<TravelOrder>(travelOrder);
        travelOrderEntity.Traffics = (ICollection<Traffic>) traffics;

        _repository.TravelOrder.CreateTravelOrder(travelOrderEntity);
        await _repository.SaveAsync();

        return _mapper.Map<TravelOrderDto>(travelOrderEntity);
    }

    public async Task UpdateTravelOrder(TravelOrderUpdateDto travelOrder, TravelOrder travelOrderEntity)
    {
        var startCity = await _repository.City.GetCityAsync(travelOrder.StartPlaceCityId, false);
        if (startCity == null)
            throw new Exception();

        var endCity = await _repository.City.GetCityAsync(travelOrder.EndPlaceCityId, false);
        if (endCity == null)
            throw new Exception();

        var employee = await _repository.Employee.GetEmployeeAsync(travelOrder.EmployeeId, false);
        if (employee == null)
            throw new Exception();

        var traffics = await _repository.Traffic.GetByIdsAsync(travelOrder.Traffics.Select(t => t.Id), true);
        if (traffics.Count() != travelOrder.Traffics.Count)
            throw new Exception();

        _mapper.Map(travelOrder, travelOrderEntity);
        travelOrderEntity.Traffics.Clear();
        travelOrderEntity.Traffics = (ICollection<Traffic>) traffics;

        await _repository.SaveAsync();
    }

    public async Task DeleteTravelOrder(TravelOrder travelOrder)
    {
        _repository.TravelOrder.DeleteTravelOrder(travelOrder);
        await _repository.SaveAsync();
    }

    [Obsolete($"Use method {nameof(GetAllTravelOrdersSelected)} instead.")]
    public async Task<(IEnumerable<TravelOrderDto>, MetaData)> GetAllTravelOrdersAsync(
        RequestParameters requestParameters,
        bool trackChanges)
    {
        var travelOrdersFromDb = await _repository.TravelOrder.GetAllTravelOrdersAsync(requestParameters, trackChanges);

        var travelOrdersDto = _mapper.Map<IEnumerable<TravelOrderDto>>(travelOrdersFromDb);

        return (travelOrdersDto, travelOrdersFromDb.MetaData);
    }

    [Obsolete($"Use endpoint {nameof(GetTravelOrderSelectedAsync)} instead.")]
    public async Task<TravelOrderDto> GetTravelOrderAsync(int id)
    {
        var travelOrder = await _repository.TravelOrder.GetTravelOrderAsync(id, trackChanges: false);
        if (travelOrder == null)
            throw new Exception();

        return _mapper.Map<TravelOrderDto>(travelOrder);
    }
}
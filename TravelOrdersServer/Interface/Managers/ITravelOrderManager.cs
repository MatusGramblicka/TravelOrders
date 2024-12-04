using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ITravelOrderManager
{
    PagedList<TravelOrderSelectedDto> GetTravelOrdersSelected(RequestParameters requestParameters);

    Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int id);

    Task<TravelOrderDto> CreateTravelOrderAsync(TravelOrderCreationDto travelOrderDto);

    Task UpdateTravelOrder(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto);

    Task UpdateTravelOrderDirectMapping(TravelOrder travelOrderFromDb, TravelOrderUpdateDto travelOrderDto);

    Task DeleteTravelOrder(TravelOrder travelOrderDb);
}
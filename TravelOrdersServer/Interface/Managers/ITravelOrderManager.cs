using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ITravelOrderManager
{
    PagedList<TravelOrderSelectedDto> GetAllTravelOrdersSelected(RequestParameters requestParameters);

    Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int id);

    Task<TravelOrderDto> CreateTravelOrderAsync(TravelOrderCreationDto travelOrder);

    Task UpdateTravelOrder(TravelOrderUpdateDto travelOrder, TravelOrder travelOrderEntity);
    
    Task DeleteTravelOrder(TravelOrder travelOrder);
}
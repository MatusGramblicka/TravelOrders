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
    
    [Obsolete($"Use method {nameof(GetAllTravelOrdersSelected)} instead.")]
    Task<(IEnumerable<TravelOrderDto>, MetaData)> GetAllTravelOrdersAsync(RequestParameters requestParameters,
        bool trackChanges);

    [Obsolete($"Use endpoint {nameof(GetTravelOrderSelectedAsync)} instead.")]
    Task<TravelOrderDto> GetTravelOrderAsync(int id);
}
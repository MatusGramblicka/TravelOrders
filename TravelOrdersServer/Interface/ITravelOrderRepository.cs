using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;

namespace Interface;

public interface ITravelOrderRepository
{
    Task<PagedList<TravelOrder>> GetAllTravelOrdersAsync(RequestParameters requestParameters,
        bool trackChanges);

    PagedList<TravelOrderSelectedDto> GetAllTravelOrdersSelectedAsync(RequestParameters requestParameters);

    Task<TravelOrder?> GetTravelOrderAsync(int travelOrdersId, bool trackChanges);

    TravelOrderSelectedDto? GetTravelOrderSelectedAsync(int travelOrdersId);

    Task<TravelOrder?> GetTravelOrderWithTrafficsAsync(int travelOrdersId);

    Task<IEnumerable<TravelOrder>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);

    void CreateTravelOrder(TravelOrder travelOrder);

    void DeleteTravelOrder(TravelOrder travelOrder);
}
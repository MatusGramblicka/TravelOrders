using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface ITravelOrderRepository
{
    PagedList<TravelOrderSelectedDto> GetTravelOrdersSelected(RequestParameters requestParameters);
    
    Task<TravelOrder?> GetTravelOrderAsync(int travelOrdersId, bool trackChanges);

    Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int travelOrdersId);

    Task<TravelOrder?> GetTravelOrderWithTrafficsAsync(int travelOrdersId);

    IQueryable<TravelOrder> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);

    void CreateTravelOrder(TravelOrder travelOrder);

    void DeleteTravelOrder(TravelOrder travelOrder);
}
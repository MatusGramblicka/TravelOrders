using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface ITrafficRepository
{
    PagedList<TrafficSelectedDto> GetTrafficsSelected(RequestParameters requestParameters);

   Task<Traffic?> GetTrafficAsync(int trafficId, bool trackChanges);

    Task<IEnumerable<Traffic>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);

    IQueryable<Traffic> GetTrafficsToSpecificTravelOrder(int travelOrderId, bool trackChanges);
}
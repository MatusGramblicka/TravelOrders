using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;

namespace Interface;

public interface ITrafficRepository
{
    Task<PagedList<Traffic>> GetAllTrafficsAsync(RequestParameters requestParameters, bool trackChanges);

    PagedList<TrafficSelectedDto> GetAllTrafficsSelectedAsync(RequestParameters requestParameters);

    Task<Traffic?> GetTrafficAsync(int trafficId, bool trackChanges);

    Task<IEnumerable<Traffic>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
}
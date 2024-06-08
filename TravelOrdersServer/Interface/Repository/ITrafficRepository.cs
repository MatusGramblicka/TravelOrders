using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface ITrafficRepository
{
    PagedList<TrafficSelectedDto> GetAllTrafficsSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllTrafficsSelected)} instead.")]
    PagedList<Traffic> GetAllTrafficsAsync(RequestParameters requestParameters, bool trackChanges);

    Task<Traffic?> GetTrafficAsync(int trafficId, bool trackChanges);

    Task<IEnumerable<Traffic>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
}
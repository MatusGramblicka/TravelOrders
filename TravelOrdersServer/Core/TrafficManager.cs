using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class TrafficManager(IRepositoryManager repository) : ITrafficManager
{
    public PagedList<TrafficSelectedDto> GetTrafficsSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        return repository.Traffic.GetTrafficsSelected(requestParameters);
    }
}
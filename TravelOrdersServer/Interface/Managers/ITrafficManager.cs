using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ITrafficManager
{
    PagedList<TrafficSelectedDto> GetAllTrafficsSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllTrafficsSelected)} instead.")]
    Task<(IEnumerable<TrafficDto>, MetaData)> GetAllTrafficsAsync(RequestParameters requestParameters,
        bool trackChanges);
}
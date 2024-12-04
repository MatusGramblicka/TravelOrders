using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ITrafficManager
{
    PagedList<TrafficSelectedDto> GetTrafficsSelected(RequestParameters requestParameters);
}
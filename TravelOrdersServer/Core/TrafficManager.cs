using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class TrafficManager : ITrafficManager
{
    private readonly IRepositoryManager _repository;

    public TrafficManager(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public PagedList<TrafficSelectedDto> GetAllTrafficsSelected(RequestParameters requestParameters)
    {
        return _repository.Traffic.GetAllTrafficsSelected(requestParameters);
    }
}
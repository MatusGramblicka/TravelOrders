using AutoMapper;
using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class TrafficManager : ITrafficManager
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public TrafficManager(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedList<TrafficSelectedDto> GetAllTrafficsSelected(RequestParameters requestParameters)
    {
        return _repository.Traffic.GetAllTrafficsSelected(requestParameters);
    }

    [Obsolete($"Use method {nameof(GetAllTrafficsSelected)} instead.")]
    public async Task<(IEnumerable<TrafficDto>, MetaData)> GetAllTrafficsAsync(RequestParameters requestParameters, bool trackChanges)
    {
        var trafficsFromDb = await _repository.Traffic.GetAllTrafficsAsync(requestParameters, trackChanges);

        var trafficsDto = _mapper.Map<IEnumerable<TrafficDto>>(trafficsFromDb);

        return (trafficsDto, trafficsFromDb.MetaData);
    }
}
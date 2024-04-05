using AutoMapper;
using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class CityManager : ICityManager
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CityManager(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedList<CitySelectedDto> GetAllCitiesSelected(RequestParameters requestParameters)
    {
        return _repository.City.GetAllCitiesSelected(requestParameters);
    }

    [Obsolete($"Use method {nameof(GetAllCitiesSelected)} instead.")]
    public async Task<(IEnumerable<CityDto>, MetaData)> GetAllCitiesAsync(RequestParameters requestParameters,
        bool trackChanges)
    {
        var citiesFromDb = await _repository.City.GetAllCitiesAsync(requestParameters, trackChanges);

        var citiesDto = _mapper.Map<IEnumerable<CityDto>>(citiesFromDb);

        return (citiesDto, citiesFromDb.MetaData);
    }
}
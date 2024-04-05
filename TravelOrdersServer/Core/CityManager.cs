using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class CityManager : ICityManager
{
    private readonly IRepositoryManager _repository;

    public CityManager(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public PagedList<CitySelectedDto> GetAllCitiesSelected(RequestParameters requestParameters)
    {
        return _repository.City.GetAllCitiesSelected(requestParameters);
    }
}
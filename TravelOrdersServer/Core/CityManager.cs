using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class CityManager(IRepositoryManager repository) : ICityManager
{
    public PagedList<CitySelectedDto> GetCitiesSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        return repository.City.GetCitiesSelected(requestParameters);
    }
}
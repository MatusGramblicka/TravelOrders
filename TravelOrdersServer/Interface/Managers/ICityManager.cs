using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ICityManager
{
    PagedList<CitySelectedDto> GetAllCitiesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllCitiesSelected)} instead.")]
    Task<(IEnumerable<CityDto>, MetaData)> GetAllCitiesAsync(RequestParameters requestParameters,
        bool trackChanges);
}
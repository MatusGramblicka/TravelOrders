using Contracts.Dto;
using Entities.RequestFeatures;

namespace Interface;

public interface ICityManager
{
    PagedList<CitySelectedDto> GetAllCitiesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllCitiesSelected)} instead.")]
    Task<(IEnumerable<CityDto>, MetaData)> GetAllCitiesAsync(RequestParameters requestParameters,
        bool trackChanges);
}
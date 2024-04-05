using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface ICityRepository
{
    PagedList<CitySelectedDto> GetAllCitiesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllCitiesSelected)} instead.")]
    Task<PagedList<City>> GetAllCitiesAsync(RequestParameters requestParameters, bool trackChanges);

    Task<City?> GetCityAsync(int cityId, bool trackChanges);

    Task<IEnumerable<City>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
}
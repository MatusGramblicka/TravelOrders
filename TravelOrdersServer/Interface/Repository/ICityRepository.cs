using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface ICityRepository
{
    PagedList<CitySelectedDto> GetCitiesSelected(RequestParameters requestParameters);

   Task<City?> GetCityAsync(int cityId, bool trackChanges);

    Task<IEnumerable<City>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
}
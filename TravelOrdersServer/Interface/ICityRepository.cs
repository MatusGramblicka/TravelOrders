using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;

namespace Interface;

public interface ICityRepository
{
    Task<PagedList<City>> GetAllCitiesAsync(RequestParameters requestParameters, bool trackChanges);

    PagedList<CitySelectedDto> GetAllCitiesSelectedAsync(RequestParameters requestParameters);

    Task<City?> GetCityAsync(int cityId, bool trackChanges);

    Task<IEnumerable<City>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
}
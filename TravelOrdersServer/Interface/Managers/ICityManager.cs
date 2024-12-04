using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface ICityManager
{
    PagedList<CitySelectedDto> GetCitiesSelected(RequestParameters requestParameters);
}
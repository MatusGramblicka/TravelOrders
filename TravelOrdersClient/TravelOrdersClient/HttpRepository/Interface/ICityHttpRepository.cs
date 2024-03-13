using Contracts.Dto;
using Entities.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface ICityHttpRepository
{
    Task<PagingResponse<CitySelectedDto>> GetCities(RequestParameters requestParameters);
}
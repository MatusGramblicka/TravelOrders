using Contracts.Dto;
using Contracts.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface ICityHttpRepository
{
    Task<PagingResponse<CitySelectedDto>> GetCities(RequestParameters requestParameters);
}
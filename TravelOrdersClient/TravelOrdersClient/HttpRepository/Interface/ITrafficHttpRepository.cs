using Contracts.Dto;
using Entities.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface ITrafficHttpRepository
{
    Task<PagingResponse<TrafficSelectedDto>> GetTraffics(RequestParameters requestParameters);

}
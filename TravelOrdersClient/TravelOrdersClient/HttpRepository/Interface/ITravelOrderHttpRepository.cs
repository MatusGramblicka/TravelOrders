using Contracts.Dto;
using Contracts.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface ITravelOrderHttpRepository
{
    Task<PagingResponse<TravelOrderSelectedDto>> GetTravelOrders(RequestParameters requestParameters);
    Task<TravelOrderSelectedDto?> GetTravelOrder(int id);
    Task CreateTravelOrder(TravelOrderCreationDto travelOrder);
    Task UpdateTravelOrder(int id, TravelOrderUpdateDto travelOrder);
    Task DeleteTravelOrder(int id);
}
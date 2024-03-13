using Contracts.Dto;
using Entities.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface IEmployeeHttpRepository
{
    Task<PagingResponse<EmployeeSelectedDto>> GetEmployees(RequestParameters requestParameters);
}
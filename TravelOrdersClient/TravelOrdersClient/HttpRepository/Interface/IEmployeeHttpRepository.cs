using Contracts.Dto;
using Contracts.RequestFeatures;
using TravelOrdersClient.Features;

namespace TravelOrdersClient.HttpRepository.Interface;

public interface IEmployeeHttpRepository
{
    Task<PagingResponse<EmployeeSelectedDto>> GetEmployees(RequestParameters requestParameters);
}
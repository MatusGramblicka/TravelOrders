using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface IEmployeeManager
{
    PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters);
}
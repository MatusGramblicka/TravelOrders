using Contracts.Dto;
using Contracts.RequestFeatures;

namespace Interface.Managers;

public interface IEmployeeManager
{
    PagedList<EmployeeSelectedDto> GetEmployeesSelected(RequestParameters requestParameters);
}
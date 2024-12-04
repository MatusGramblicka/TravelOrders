using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class EmployeeManager(IRepositoryManager repository) : IEmployeeManager
{
    public PagedList<EmployeeSelectedDto> GetEmployeesSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        return repository.Employee.GetEmployeesSelected(requestParameters);
    }
}
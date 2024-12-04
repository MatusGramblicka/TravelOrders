using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface IEmployeeRepository
{
    PagedList<EmployeeSelectedDto> GetEmployeesSelected(RequestParameters requestParameters);
    
    Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges);

    Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges);
}
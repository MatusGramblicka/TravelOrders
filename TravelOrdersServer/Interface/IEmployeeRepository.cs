using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;

namespace Interface;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetAllEmployeesAsync(RequestParameters requestParameters, bool trackChanges);

    PagedList<EmployeeSelectedDto> GetAllEmployeesSelectedAsync(RequestParameters requestParameters);

    Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges);

    Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges);
}
using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;

namespace Interface.Repository;

public interface IEmployeeRepository
{
    PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters);

    [Obsolete($"Use method {nameof(GetAllEmployeesSelected)} instead.")]
    PagedList<Employee> GetAllEmployeesAsync(RequestParameters requestParameters, bool trackChanges);

    Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges);

    Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges);
}
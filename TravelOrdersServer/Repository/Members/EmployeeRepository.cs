using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Members;

public class EmployeeRepository(TravelOrderDbContext repositoryContext)
    : RepositoryBase<Employee>(repositoryContext), IEmployeeRepository
{
    public PagedList<EmployeeSelectedDto> GetEmployeesSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        var employees = RepositoryContext.Employee.Select(e => new EmployeeSelectedDto
        {
            Id = e.Id,
            Name = e.Name,
            Surname = e.Surname
        });

        return PagedList<EmployeeSelectedDto>
            .ToPagedList(employees, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(employeesId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();
}
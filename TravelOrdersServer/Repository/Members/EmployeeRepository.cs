using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Members;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters)
    {
        var employees = RepositoryContext.Employee.Select(e => new EmployeeSelectedDto
        {
            Id = e.Id,
            Name = e.Name,
            Surname = e.Surname
        });

        return PagedList<EmployeeSelectedDto>
            .ToPagedList(employees, requestParameters.PageNumber, requestParameters.PageSize);
    }

    [Obsolete($"Use method {nameof(GetAllEmployeesSelected)} instead.")]
    public async Task<PagedList<Employee>> GetAllEmployeesAsync(RequestParameters requestParameters, bool trackChanges)
    {
        var employees = await FindAll(trackChanges)
            .ToListAsync();

        return PagedList<Employee>
            .ToPagedList(employees, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Employee?> GetEmployeeAsync(string employeesId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(employeesId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<string> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();
}
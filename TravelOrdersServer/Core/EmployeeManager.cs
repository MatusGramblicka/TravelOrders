using Contracts.Dto;
using Contracts.RequestFeatures;
using Interface.DatabaseAccess;
using Interface.Managers;

namespace Core;

public class EmployeeManager : IEmployeeManager
{
    private readonly IRepositoryManager _repository;

    public EmployeeManager(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters)
    {
        return _repository.Employee.GetAllEmployeesSelected(requestParameters);
    }
}
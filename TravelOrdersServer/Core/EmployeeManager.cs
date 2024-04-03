using AutoMapper;
using Contracts.Dto;
using Entities.RequestFeatures;
using Interface;

namespace Core;

public class EmployeeManager : IEmployeeManager
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public EmployeeManager(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedList<EmployeeSelectedDto> GetAllEmployeesSelected(RequestParameters requestParameters)
    {
        return _repository.Employee.GetAllEmployeesSelected(requestParameters);
    }

    [Obsolete($"Use method {nameof(GetAllEmployeesSelected)} instead.")]
    public async Task<(IEnumerable<EmployeeDto>, MetaData)> GetAllEmployeesAsync(RequestParameters requestParameters,
        bool trackChanges)
    {
        var employeesFromDb = await _repository.Employee.GetAllEmployeesAsync(requestParameters, trackChanges);

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return (employeesDto, employeesFromDb.MetaData);
    }
}
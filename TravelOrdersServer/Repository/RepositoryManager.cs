using Interface;
using Repository.Members;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly TravelOrderDbContext _repositoryContext;
    private ICityRepository _cityRepository;
    private IEmployeeRepository _employeeRepository;
    private ITravelOrderRepository _travelOrderRepository;
    private ITrafficRepository _trafficRepository;

    public RepositoryManager(TravelOrderDbContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public ICityRepository City
    {
        get { return _cityRepository ??= new CityRepository(_repositoryContext); }
    }

    public IEmployeeRepository Employee
    {
        get { return _employeeRepository ??= new EmployeeRepository(_repositoryContext); }
    }

    public ITravelOrderRepository TravelOrder
    {
        get { return _travelOrderRepository ??= new TravelOrderRepository(_repositoryContext); }
    }

    public ITrafficRepository Traffic
    {
        get { return _trafficRepository ??= new TrafficRepository(_repositoryContext); }
    }
    

    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}
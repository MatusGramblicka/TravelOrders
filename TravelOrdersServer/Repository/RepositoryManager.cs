using Interface.DatabaseAccess;
using Interface.Repository;
using Repository.Members;

namespace Repository;

public class RepositoryManager(TravelOrderDbContext repositoryContext) : IRepositoryManager
{
    private ICityRepository _cityRepository;
    private IEmployeeRepository _employeeRepository;
    private ITravelOrderRepository _travelOrderRepository;
    private ITrafficRepository _trafficRepository;

    public ICityRepository City
    {
        get { return _cityRepository ??= new CityRepository(repositoryContext); }
    }

    public IEmployeeRepository Employee
    {
        get { return _employeeRepository ??= new EmployeeRepository(repositoryContext); }
    }

    public ITravelOrderRepository TravelOrder
    {
        get { return _travelOrderRepository ??= new TravelOrderRepository(repositoryContext); }
    }

    public ITrafficRepository Traffic
    {
        get { return _trafficRepository ??= new TrafficRepository(repositoryContext); }
    }
    
    public Task SaveAsync() => repositoryContext.SaveChangesAsync();
}
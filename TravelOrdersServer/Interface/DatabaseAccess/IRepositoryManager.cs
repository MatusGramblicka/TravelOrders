using Interface.Repository;

namespace Interface.DatabaseAccess;

public interface IRepositoryManager
{
    ICityRepository City { get; }

    IEmployeeRepository Employee { get; }

    ITravelOrderRepository TravelOrder { get; }

    ITrafficRepository Traffic { get; }

    Task SaveAsync();
}
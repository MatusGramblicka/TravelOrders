using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Members;

public class TrafficRepository : RepositoryBase<Traffic>, ITrafficRepository
{
    public TrafficRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public PagedList<TrafficSelectedDto> GetAllTrafficsSelected(RequestParameters requestParameters)
    {
        var traffics = RepositoryContext.Traffic.Select(t => new TrafficSelectedDto
        {
            Id = t.Id,
            TrafficDevice = t.TrafficDevice
        });

        return PagedList<TrafficSelectedDto>
            .ToPagedList(traffics, requestParameters.PageNumber, requestParameters.PageSize);
    }

    [Obsolete($"Use method {nameof(GetAllTrafficsSelected)} instead.")]
    public PagedList<Traffic> GetAllTrafficsAsync(RequestParameters requestParameters, bool trackChanges)
    {
        var traffics = FindAll(trackChanges);

        return PagedList<Traffic>
            .ToPagedList(traffics, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Traffic?> GetTrafficAsync(int trafficId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(trafficId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Traffic>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

    public List<Traffic> GetAllTrafficsToSpecificTravelOrder(int travelOrderId, bool trackChanges)
    {
        return RepositoryContext.TravelOrder
            .Where(o => o.Id == travelOrderId)
            .SelectMany(s=>s.Traffics)
            .Distinct()
            .ToList();
    }
}
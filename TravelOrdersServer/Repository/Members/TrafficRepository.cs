using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository.Members;

public class TrafficRepository(TravelOrderDbContext repositoryContext)
    : RepositoryBase<Traffic>(repositoryContext), ITrafficRepository
{
    public PagedList<TrafficSelectedDto> GetTrafficsSelected(RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        var traffics = RepositoryContext.Traffic.Select(t => new TrafficSelectedDto
        {
            Id = t.Id,
            TrafficDevice = t.TrafficDevice
        });

        return PagedList<TrafficSelectedDto>
            .ToPagedList(traffics, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<Traffic?> GetTrafficAsync(int trafficId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(trafficId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Traffic>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

    public IQueryable<Traffic> GetTrafficsToSpecificTravelOrder(int travelOrderId, bool trackChanges)
    {
        return RepositoryContext.TravelOrder
            .Where(o => o.Id == travelOrderId)
            .SelectMany(s => s.Traffics)
            .Distinct();
    }
}
using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;
using Interface;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class TrafficRepository : RepositoryBase<Traffic>, ITrafficRepository
{
    public TrafficRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<PagedList<Traffic>> GetAllTrafficsAsync(RequestParameters requestParameters, bool trackChanges)
    {
        var traffics = await FindAll(trackChanges)
            .ToListAsync();

        return PagedList<Traffic>
            .ToPagedList(traffics, requestParameters.PageNumber, requestParameters.PageSize);
    }


    public PagedList<TrafficSelectedDto> GetAllTrafficsSelectedAsync(RequestParameters requestParameters)
    {
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
}
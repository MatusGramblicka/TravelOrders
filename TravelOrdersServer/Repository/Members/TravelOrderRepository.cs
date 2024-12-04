using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Repository.Projections;

namespace Repository.Members;

public class TravelOrderRepository(TravelOrderDbContext repositoryContext)
    : RepositoryBase<TravelOrder>(repositoryContext), ITravelOrderRepository
{
    public PagedList<TravelOrderSelectedDto> GetTravelOrdersSelected(
        RequestParameters requestParameters)
    {
        ArgumentNullException.ThrowIfNull(requestParameters, nameof(requestParameters));

        var travelOrders = RepositoryContext.TravelOrder
            .Select(TravelOrderProjection.GetTravelOrderSelected())
            .Search(requestParameters.SearchTerm);

        return PagedList<TravelOrderSelectedDto>
            .ToPagedList(travelOrders, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<TravelOrder?> GetTravelOrderAsync(int travelOrdersId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(travelOrdersId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<TravelOrderSelectedDto?> GetTravelOrderSelectedAsync(int travelOrdersId)
    {
        return await RepositoryContext.TravelOrder
            .Where(i => i.Id.Equals(travelOrdersId))
            .Select(TravelOrderProjection.GetTravelOrderSelected())
            .SingleOrDefaultAsync();
    }

    public async Task<TravelOrder?> GetTravelOrderWithTrafficsAsync(int travelOrdersId)
    {
        return await RepositoryContext.TravelOrder
            .Include(i => i.Traffics)
            .Where(c => c.Id.Equals(travelOrdersId))
            .SingleOrDefaultAsync();
    }

    public IQueryable<TravelOrder> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
    {
        ArgumentNullException.ThrowIfNull(ids, nameof(ids));

        return FindByCondition(x => ids.Contains(x.Id), trackChanges);
    }

    public void CreateTravelOrder(TravelOrder travelOrder) => Create(travelOrder);

    public void DeleteTravelOrder(TravelOrder travelOrder)
    {
        Delete(travelOrder);
    }
}
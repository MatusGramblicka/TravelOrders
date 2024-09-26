using Contracts.Dto;
using Contracts.Models;
using Contracts.RequestFeatures;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Repository.Projections;

namespace Repository.Members;

public class TravelOrderRepository : RepositoryBase<TravelOrder>, ITravelOrderRepository
{
    public TravelOrderRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }
    
    public PagedList<TravelOrderSelectedDto> GetAllTravelOrdersSelected(
        RequestParameters requestParameters)
    {
        if (requestParameters == null)
            throw new Exception();

        var travelOrders = RepositoryContext.TravelOrder
            .Select(TravelOrderProjection.GetTravelOrderSelected())
            .Search(requestParameters.SearchTerm);

        return PagedList<TravelOrderSelectedDto>
            .ToPagedList(travelOrders, requestParameters.PageNumber, requestParameters.PageSize);
    }

    [Obsolete($"Use method {nameof(GetAllTravelOrdersSelected)} instead.")]
    public PagedList<TravelOrder> GetAllTravelOrdersAsync(RequestParameters requestParameters,
        bool trackChanges)
    {
        if (requestParameters == null)
            throw new Exception();

        var travelOrders = FindAll(trackChanges);

        return PagedList<TravelOrder>
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

    public async Task<IEnumerable<TravelOrder>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();

    public void CreateTravelOrder(TravelOrder travelOrder) => Create(travelOrder);

    public void DeleteTravelOrder(TravelOrder travelOrder)
    {
        Delete(travelOrder);
    }
}
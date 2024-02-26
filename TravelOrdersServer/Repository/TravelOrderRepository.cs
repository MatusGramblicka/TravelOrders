using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;
using Interface;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository;

public class TravelOrderRepository : RepositoryBase<TravelOrder>, ITravelOrderRepository
{
    public TravelOrderRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<PagedList<TravelOrder>> GetAllTravelOrdersAsync(RequestParameters requestParameters,
        bool trackChanges)
    {
        var travelOrders = await FindAll(trackChanges)
            .ToListAsync();

        return PagedList<TravelOrder>
            .ToPagedList(travelOrders, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public PagedList<TravelOrderSelectedDto> GetAllTravelOrdersSelectedAsync(
        RequestParameters requestParameters)
    {
        var travelOrders = RepositoryContext.TravelOrder.Select(t => new TravelOrderSelectedDto
        {
            Id = t.Id,
            State = t.State,
            EndPlace = new CitySelectedDto
            {
                State = t.EndPlace.State,
                Id = t.EndPlace.Id,
                Name = t.EndPlace.Name
            },
            StartPlace = new CitySelectedDto
            {
                State = t.StartPlace.State,
                Id = t.StartPlace.Id,
                Name = t.StartPlace.Name
            },
            StartDate = t.StartDate,
            CreatedAt = t.CreatedAt,
            EndDate = t.EndDate,
            Tenant = new EmployeeSelectedDto
            {
                Id = t.Tenant.Id,
                Name = t.Tenant.Name,
                Surname = t.Tenant.Surname
            },
            Traffics = t.Traffics.Select(tf => new TrafficSelectedDto
            {
                Id = tf.Id,
                TrafficDevice = tf.TrafficDevice
            }).ToList()
        }).Search(requestParameters.SearchTerm);

        return PagedList<TravelOrderSelectedDto>
            .ToPagedList(travelOrders, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<TravelOrder?> GetTravelOrderAsync(int travelOrdersId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(travelOrdersId), trackChanges)
            .SingleOrDefaultAsync();

    public TravelOrderSelectedDto? GetTravelOrderSelectedAsync(int travelOrdersId)
    {
        return RepositoryContext.TravelOrder
            .Where(i => i.Id.Equals(travelOrdersId))
            .Select(t => new TravelOrderSelectedDto
            {
                Id = t.Id,
                State = t.State,
                EndPlace = new CitySelectedDto
                {
                    State = t.EndPlace.State,
                    Id = t.EndPlace.Id,
                    Name = t.EndPlace.Name
                },
                StartPlace = new CitySelectedDto
                {
                    State = t.StartPlace.State,
                    Id = t.StartPlace.Id,
                    Name = t.StartPlace.Name
                },
                StartDate = t.StartDate,
                CreatedAt = t.CreatedAt,
                EndDate = t.EndDate,
                Tenant = new EmployeeSelectedDto
                {
                    Id = t.Tenant.Id,
                    Name = t.Tenant.Name,
                    Surname = t.Tenant.Surname
                },
                Traffics = t.Traffics.Select(tf => new TrafficSelectedDto
                {
                    Id = tf.Id,
                    TrafficDevice = tf.TrafficDevice
                }).ToList()
            }).SingleOrDefault();
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
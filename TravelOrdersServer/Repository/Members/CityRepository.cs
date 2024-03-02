using Contracts.Dto;
using Contracts.Models;
using Entities.RequestFeatures;
using Interface;
using Microsoft.EntityFrameworkCore;

namespace Repository.Members;

public class CityRepository : RepositoryBase<City>, ICityRepository
{
    public CityRepository(TravelOrderDbContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<PagedList<City>> GetAllCitiesAsync(RequestParameters requestParameters, bool trackChanges)
    {
        var cities = await FindAll(trackChanges)
            .ToListAsync();

        return PagedList<City>
            .ToPagedList(cities, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public PagedList<CitySelectedDto> GetAllCitiesSelectedAsync(RequestParameters requestParameters)
    {
        var cities = RepositoryContext.City.Select(c => new CitySelectedDto
        {
            Id = c.Id,
            State = c.State,
            Name = c.Name
        });

        return PagedList<CitySelectedDto>
            .ToPagedList(cities, requestParameters.PageNumber, requestParameters.PageSize);
    }

    public async Task<City?> GetCityAsync(int cityId, bool trackChanges) =>
        await FindByCondition(c => c.Id.Equals(cityId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<City>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToListAsync();
}
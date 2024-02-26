using Contracts.Dto;

namespace Repository.Extensions;

public static class RepositoryTravelOrderExtensions
{
    public static IQueryable<TravelOrderSelectedDto> Search(this IQueryable<TravelOrderSelectedDto> travelOrders,
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return travelOrders;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return travelOrders.Where(t => t.Tenant.Surname.ToLower().Contains(lowerCaseTerm));
    }
}
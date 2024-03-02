using System.Linq.Expressions;
using Contracts.Dto;
using Contracts.Models;

namespace Repository.Projections;

public static class TravelOrderProjection
{
    public static Expression<Func<TravelOrder,TravelOrderSelectedDto>> GetTravelOrderSelected()
    {
        return travelOrder => new TravelOrderSelectedDto
        {
            Id = travelOrder.Id,
            State = travelOrder.State,
            EndPlace = new CitySelectedDto
            {
                State = travelOrder.EndPlace.State,
                Id = travelOrder.EndPlace.Id,
                Name = travelOrder.EndPlace.Name
            },
            StartPlace = new CitySelectedDto
            {
                State = travelOrder.StartPlace.State,
                Id = travelOrder.StartPlace.Id,
                Name = travelOrder.StartPlace.Name
            },
            StartDate = travelOrder.StartDate,
            CreatedAt = travelOrder.CreatedAt,
            EndDate = travelOrder.EndDate,
            Tenant = new EmployeeSelectedDto
            {
                Id = travelOrder.Tenant.Id,
                Name = travelOrder.Tenant.Name,
                Surname = travelOrder.Tenant.Surname
            },
            Traffics = travelOrder.Traffics.Select(tf => new TrafficSelectedDto
            {
                Id = tf.Id,
                TrafficDevice = tf.TrafficDevice
            }).ToList()
        };
    }
}
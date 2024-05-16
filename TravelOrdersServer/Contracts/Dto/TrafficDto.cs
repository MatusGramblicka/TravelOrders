using Contracts.Enums;
using Contracts.Models;

namespace Contracts.Dto;

public class TrafficDto
{
    public int Id { get; set; }

    public TrafficDevice TrafficDevice { get; set; }

    public ICollection<TravelOrder> TravelOrders { get; set; } = new List<TravelOrder>();
}
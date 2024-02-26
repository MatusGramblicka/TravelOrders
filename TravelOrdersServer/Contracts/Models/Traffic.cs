using Contracts.Enums;

namespace Contracts.Models;

public class Traffic
{
    public int Id { get; set; }
    public TrafficDevice TrafficDevice { get; set; }
    public ICollection<TravelOrder> TravelOrders { get; set; } = new List<TravelOrder>();
}
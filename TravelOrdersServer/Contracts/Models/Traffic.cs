using Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Models;

public class Traffic
{
    public int Id { get; set; }

    [MaxLength(30)]
    public TrafficDevice TrafficDevice { get; set; }

    public ICollection<TravelOrder> TravelOrders { get; set; } = new List<TravelOrder>();
}
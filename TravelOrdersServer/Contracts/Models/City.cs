using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Contracts.Models;

public class City
{
    public int Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [MaxLength(30)] 
    public string State { get; set; } = null!;

    public Point GeographicalCoordinates { get; set; } = null!;

    public ICollection<TravelOrder> StartPlaceTravelOrders { get; set; } = new List<TravelOrder>();
    public ICollection<TravelOrder> EndPlaceTravelOrders { get; set; } = new List<TravelOrder>();
}
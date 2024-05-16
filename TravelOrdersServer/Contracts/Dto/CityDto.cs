using Contracts.Models;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace Contracts.Dto;

public class CityDto
{
    public int Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(30)]
    public string State { get; set; }

    public Point GeographicalCoordinates { get; set; }

    public ICollection<TravelOrder> StartPlaceTravelOrders { get; set; } = new List<TravelOrder>();

    public ICollection<TravelOrder> EndPlaceTravelOrders { get; set; } = new List<TravelOrder>();
}
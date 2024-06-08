using Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto;

public class TravelOrderSelectedDto
{
    public int Id { get; set; }

    public DateOnly CreatedAt { get; set; }

    public EmployeeSelectedDto Tenant { get; set; }

    public CitySelectedDto StartPlace { get; set; }

    public CitySelectedDto EndPlace { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [MaxLength(60)]
    public string? Note { get; set; }

    public IEnumerable<TrafficSelectedDto> Traffics { get; set; } = new List<TrafficSelectedDto>();

    public State State { get; set; }
}
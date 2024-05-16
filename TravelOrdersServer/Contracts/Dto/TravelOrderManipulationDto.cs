using Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto;

public class TravelOrderManipulationDto
{
    [MaxLength(10)]
    public string EmployeeId { get; set; }

    public int StartPlaceCityId { get; set; }

    public int EndPlaceCityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [MaxLength(60)]
    public string? Note { get; set; }

    public ICollection<TrafficSelectedDto> Traffics { get; set; } = new List<TrafficSelectedDto>();

    public State State { get; set; }
}
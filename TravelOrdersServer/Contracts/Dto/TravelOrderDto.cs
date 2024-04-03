using Contracts.Enums;
using Contracts.Models;

namespace Contracts.Dto;

public class TravelOrderDto
{
    public int Id { get; set; }
    public DateOnly CreatedAt { get; set; }
    public Employee Tenant { get; set; }
    public City StartPlace { get; set; }
    public City EndPlace { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Traffic> Traffics { get; set; } = new List<Traffic>();
    public State State { get; set; }
}
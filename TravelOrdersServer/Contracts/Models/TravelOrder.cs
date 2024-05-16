using Contracts.Enums;

namespace Contracts.Models;

public class TravelOrder
{
    public int Id { get; set; }

    public DateOnly CreatedAt { get; set; }

    public Employee Tenant { get; set; } = null!;
    public string EmployeeId { get; set; } = null!;

    public City StartPlace { get; set; } = null!;
    public int StartPlaceCityId { get; set; }

    public City EndPlace { get; set; } = null!;
    public int EndPlaceCityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Note { get; set; }

    public ICollection<Traffic> Traffics { get; set; } = new List<Traffic>();

    public State State { get; set; }
}
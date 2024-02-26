using Contracts.Enums;

namespace Contracts.Models;

public class TravelOrder
{
    public int Id { get; set; }

    public DateOnly CreatedAt { get; set; }

    public Employee Tenant { get; set; }
    public string EmployeeId { get; set; }

    public City StartPlace { get; set; }
    public int StartPlaceCityId { get; set; }

    public City EndPlace { get; set; }
    public int EndPlaceCityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ICollection<Traffic> Traffics { get; set; } = new List<Traffic>();

    public State State { get; set; }
}
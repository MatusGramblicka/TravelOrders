using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Models;

public class Employee
{
    [Key, MaxLength(10), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;

    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string Surname { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    [MaxLength(20)]
    public string BirthNumber { get; set; } = null!;

    public ICollection<TravelOrder> TravelOrders { get; set; } = new List<TravelOrder>();
}
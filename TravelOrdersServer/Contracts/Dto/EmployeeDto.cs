using Contracts.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto;

public class EmployeeDto
{
    [Key, MaxLength(10), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Surname { get; set; }

    public DateTime BirthDate { get; set; }

    [MaxLength(20)]
    public string BirthNumber { get; set; }

    public ICollection<TravelOrder> TravelOrders { get; set; } = new List<TravelOrder>();
}
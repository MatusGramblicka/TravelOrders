using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto;

public class EmployeeSelectedDto
{
    public string Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Surname { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto;

public class CitySelectedDto
{
    public int Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(30)]
    public string State { get; set; }
}
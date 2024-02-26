using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Contracts.Models;

namespace Repository.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasData
        (
            new Employee
            {
                Id = "0102F7091D",
                Name = "Jane",
                Surname = "Legue",
                BirthDate = new DateTime(1999, 5,11),
                BirthNumber = "990511/7896"
            },
            new Employee
            {
                Id = "1109F7061A",
                Name = "Lui",
                Surname = "Pale",
                BirthDate = new DateTime(1989, 2, 1),
                BirthNumber = "890201/9896"
            },
            new Employee
            {
                Id = "8802FT0989",
                Name = "Allen",
                Surname = "Rogue",
                BirthDate = new DateTime(2000, 12, 12),
                BirthNumber = "001212/6326"
            }
        );
    }
}
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
            },
            new Employee
            {
                Id = "9202FZ0912",
                Name = "Matheus",
                Surname = "Langus",
                BirthDate = new DateTime(1969, 1, 2),
                BirthNumber = "000102/0026"
            },
            new Employee
            {
                Id = "8202FT8889",
                Name = "John",
                Surname = "Paul",
                BirthDate = new DateTime(2001, 8, 12),
                BirthNumber = "000812/6316"
            },
            new Employee
            {
                Id = "9202AD0892",
                Name = "Lys",
                Surname = "Lorence",
                BirthDate = new DateTime(1969, 10, 3),
                BirthNumber = "001003/6369"
            }
        );
    }
}
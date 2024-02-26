using Contracts.Enums;
using Contracts.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Repository.Configuration;

public class TravelOrderConfiguration : IEntityTypeConfiguration<TravelOrder>
{
    public void Configure(EntityTypeBuilder<TravelOrder> builder)
    {
        builder.HasData
        (
            new TravelOrder
            {
                Id = 1,
                EmployeeId = "1109F7061A",
                StartPlaceCityId = 3,
                EndPlaceCityId = 1,
                State = State.Created,
                StartDate = new DateTime(2024, 2, 2),
                EndDate = new DateTime(2024, 2, 4)
            }
        );
    }
}
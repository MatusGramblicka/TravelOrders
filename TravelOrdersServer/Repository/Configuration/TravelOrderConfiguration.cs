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
                EndDate = new DateTime(2024, 2, 4),
                Note = "This travel order must be processed as soon as possible."
            },
            new TravelOrder
            {
                Id = 2,
                EmployeeId = "8202FT8889",
                StartPlaceCityId = 2,
                EndPlaceCityId = 2,
                State = State.Accounted,
                StartDate = new DateTime(2024, 3, 8),
                EndDate = new DateTime(2024, 3, 12)
            }
        );
    }
}
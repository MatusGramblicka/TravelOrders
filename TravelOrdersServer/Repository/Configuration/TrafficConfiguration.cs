using Contracts.Enums;
using Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class TrafficConfiguration : IEntityTypeConfiguration<Traffic>
{
    public void Configure(EntityTypeBuilder<Traffic> builder)
    {
        builder.HasData
        (
            new Traffic
            {
                Id = 1,
                TrafficDevice = TrafficDevice.Bus
            },
            new Traffic
            {
                Id = 2,
                TrafficDevice = TrafficDevice.CompanyCar
            },
            new Traffic
            {
                Id = 3,
                TrafficDevice = TrafficDevice.Plane
            },
            new Traffic
            {
                Id = 4,
                TrafficDevice = TrafficDevice.PublicBus
            },
            new Traffic
            {
                Id = 5,
                TrafficDevice = TrafficDevice.Taxi
            },
            new Traffic
            {
                Id = 6,
                TrafficDevice = TrafficDevice.Train
            },
            new Traffic
            {
                Id = 7,
                TrafficDevice = TrafficDevice.Walk
            }
        );
    }
}
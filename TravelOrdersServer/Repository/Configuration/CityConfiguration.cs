using Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;

namespace Repository.Configuration;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasData
        (
            new City
            {
                Id = 1,
                Name = "Lyon",
                GeographicalCoordinates = new Point(new Coordinate(22,33)) { SRID = 4326 },
                State = "Francais"
            },
            new City
            {
                Id = 2,
                Name = "Trenčín",
                GeographicalCoordinates = new Point(new Coordinate(88, 25)) { SRID = 4326 },
                State = "Slovakia"
            },
            new City
            {
                Id = 3,
                Name = "Madrid",
                GeographicalCoordinates = new Point(new Coordinate(60, 45)) { SRID = 4326 },
                State = "Spain"
            }
        );
    }
}
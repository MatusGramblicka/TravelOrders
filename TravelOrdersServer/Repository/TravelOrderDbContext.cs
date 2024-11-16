using Contracts.Enums;
using Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository;

public class TravelOrderDbContext(DbContextOptions<TravelOrderDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employee { get; set; } = null!;
    public DbSet<City> City { get; set; } = null!;
    public DbSet<TravelOrder> TravelOrder { get; set; } = null!;
    public DbSet<Traffic> Traffic { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Traffic>(entity =>
            entity.Property(e => e.TrafficDevice).HasConversion(
                v => v.ToString(),
                v => !string.IsNullOrWhiteSpace(v)
                    ? (TrafficDevice) Enum.Parse(typeof(TrafficDevice), v)
                    : TrafficDevice.Bus));

        modelBuilder.Entity<TravelOrder>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql(/*"getdate()"*/"CURRENT_DATE");

        modelBuilder.Entity<TravelOrder>(entity =>
            entity.Property(e => e.State).HasConversion(
                v => v.ToString(),
                v => !string.IsNullOrWhiteSpace(v)
                    ? (State) Enum.Parse(typeof(State), v)
                    : State.Created));

        modelBuilder.Entity<TravelOrder>()
            .HasOne(t => t.Tenant)
            .WithMany(e => e.TravelOrders)
            .HasForeignKey(f => f.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
      
        modelBuilder.Entity<TravelOrder>()
            .HasOne(g => g.StartPlace)
            .WithMany(t => t.StartPlaceTravelOrders)
            .HasForeignKey(f => f.StartPlaceCityId)
            .HasPrincipalKey(t => t.Id)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TravelOrder>()
            .HasOne(g => g.EndPlace)
            .WithMany(t => t.EndPlaceTravelOrders)
            .HasForeignKey(f => f.EndPlaceCityId)
            .HasPrincipalKey(t => t.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TravelOrder>()
            .Property(p => p.StartDate)
            .HasConversion
            (
                src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
            );
        modelBuilder.Entity<TravelOrder>()
            .Property(p => p.EndDate)
            .HasConversion
            (
                src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
            );

        modelBuilder.Entity<Employee>()
            .Property(p => p.BirthDate)
            .HasConversion
            (
                src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
            );

        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new TrafficConfiguration());
        modelBuilder.ApplyConfiguration(new TravelOrderConfiguration());

        modelBuilder.Entity<TravelOrder>()
            .HasMany(p => p.Traffics)
            .WithMany(p => p.TravelOrders)
            .UsingEntity(j => j.ToTable("TrafficTravelOrder")
                .HasData(new[]
                    {
                        new {TravelOrdersId = 1, TrafficsId = 1},
                        new {TravelOrdersId = 1, TrafficsId = 2}
                    }
                ));

        base.OnModelCreating(modelBuilder);
    }
}
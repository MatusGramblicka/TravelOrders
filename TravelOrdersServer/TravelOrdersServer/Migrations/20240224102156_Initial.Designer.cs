﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Repository;

#nullable disable

namespace TravelOrdersServer.Migrations
{
    [DbContext(typeof(TravelOrderDbContext))]
    [Migration("20240224102156_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Contracts.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Point>("GeographicalCoordinates")
                        .IsRequired()
                        .HasColumnType("geography");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("City");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GeographicalCoordinates = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (22 33)"),
                            Name = "Lyon",
                            State = "Francais"
                        },
                        new
                        {
                            Id = 2,
                            GeographicalCoordinates = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (88 25)"),
                            Name = "Trenčín",
                            State = "Slovakia"
                        },
                        new
                        {
                            Id = 3,
                            GeographicalCoordinates = (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (60 45)"),
                            Name = "Madrid",
                            State = "Spain"
                        });
                });

            modelBuilder.Entity("Contracts.Models.Employee", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BirthNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            Id = "0102F7091D",
                            BirthDate = new DateTime(1999, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            BirthNumber = "990511/7896",
                            Name = "Jane",
                            Surname = "Legue"
                        },
                        new
                        {
                            Id = "1109F7061A",
                            BirthDate = new DateTime(1989, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            BirthNumber = "890201/9896",
                            Name = "Lui",
                            Surname = "Pale"
                        },
                        new
                        {
                            Id = "8802FT0989",
                            BirthDate = new DateTime(2000, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            BirthNumber = "001212/6326",
                            Name = "Allen",
                            Surname = "Rogue"
                        });
                });

            modelBuilder.Entity("Contracts.Models.Traffic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("TrafficDevice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Traffic");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TrafficDevice = "Bus"
                        },
                        new
                        {
                            Id = 2,
                            TrafficDevice = "CompanyCar"
                        },
                        new
                        {
                            Id = 3,
                            TrafficDevice = "Plane"
                        },
                        new
                        {
                            Id = 4,
                            TrafficDevice = "PublicBus"
                        },
                        new
                        {
                            Id = 5,
                            TrafficDevice = "Taxi"
                        },
                        new
                        {
                            Id = 6,
                            TrafficDevice = "Train"
                        },
                        new
                        {
                            Id = 7,
                            TrafficDevice = "Walk"
                        });
                });

            modelBuilder.Entity("Contracts.Models.TravelOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EndPlaceCityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StartPlaceCityId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EndPlaceCityId");

                    b.HasIndex("StartPlaceCityId");

                    b.ToTable("TravelOrder");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateOnly(1, 1, 1),
                            EmployeeId = "1109F7061A",
                            EndDate = new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EndPlaceCityId = 1,
                            StartDate = new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            StartPlaceCityId = 3,
                            State = "Created"
                        });
                });

            modelBuilder.Entity("TrafficTravelOrder", b =>
                {
                    b.Property<int>("TrafficsId")
                        .HasColumnType("int");

                    b.Property<int>("TravelOrdersId")
                        .HasColumnType("int");

                    b.HasKey("TrafficsId", "TravelOrdersId");

                    b.HasIndex("TravelOrdersId");

                    b.ToTable("TrafficTravelOrder", (string)null);

                    b.HasData(
                        new
                        {
                            TrafficsId = 1,
                            TravelOrdersId = 1
                        },
                        new
                        {
                            TrafficsId = 2,
                            TravelOrdersId = 1
                        });
                });

            modelBuilder.Entity("Contracts.Models.TravelOrder", b =>
                {
                    b.HasOne("Contracts.Models.Employee", "Tenant")
                        .WithMany("TravelOrders")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Contracts.Models.City", "EndPlace")
                        .WithMany("EndPlaceTravelOrders")
                        .HasForeignKey("EndPlaceCityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Contracts.Models.City", "StartPlace")
                        .WithMany("StartPlaceTravelOrders")
                        .HasForeignKey("StartPlaceCityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EndPlace");

                    b.Navigation("StartPlace");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("TrafficTravelOrder", b =>
                {
                    b.HasOne("Contracts.Models.Traffic", null)
                        .WithMany()
                        .HasForeignKey("TrafficsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contracts.Models.TravelOrder", null)
                        .WithMany()
                        .HasForeignKey("TravelOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Contracts.Models.City", b =>
                {
                    b.Navigation("EndPlaceTravelOrders");

                    b.Navigation("StartPlaceTravelOrders");
                });

            modelBuilder.Entity("Contracts.Models.Employee", b =>
                {
                    b.Navigation("TravelOrders");
                });
#pragma warning restore 612, 618
        }
    }
}

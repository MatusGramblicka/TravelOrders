﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelOrdersServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    GeographicalCoordinates = table.Column<Point>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BirthNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Traffic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrafficDevice = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traffic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TravelOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    EmployeeId = table.Column<string>(type: "character varying(10)", nullable: false),
                    StartPlaceCityId = table.Column<int>(type: "integer", nullable: false),
                    EndPlaceCityId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    State = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TravelOrder_City_EndPlaceCityId",
                        column: x => x.EndPlaceCityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelOrder_City_StartPlaceCityId",
                        column: x => x.StartPlaceCityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelOrder_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrafficTravelOrder",
                columns: table => new
                {
                    TrafficsId = table.Column<int>(type: "integer", nullable: false),
                    TravelOrdersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficTravelOrder", x => new { x.TrafficsId, x.TravelOrdersId });
                    table.ForeignKey(
                        name: "FK_TrafficTravelOrder_Traffic_TrafficsId",
                        column: x => x.TrafficsId,
                        principalTable: "Traffic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrafficTravelOrder_TravelOrder_TravelOrdersId",
                        column: x => x.TravelOrdersId,
                        principalTable: "TravelOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "GeographicalCoordinates", "Name", "State" },
                values: new object[,]
                {
                    { 1, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (22 33)"), "Lyon", "Francais" },
                    { 2, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (88 25)"), "Trenčín", "Slovakia" },
                    { 3, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (60 45)"), "Madrid", "Spain" },
                    { 4, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (50 12)"), "New York", "America" },
                    { 5, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (70 18)"), "Quebec", "Canada" },
                    { 6, (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (49 30)"), "Praha", "Czech Republic" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "BirthDate", "BirthNumber", "Name", "Surname" },
                values: new object[,]
                {
                    { "0102F7091D", new DateTime(1999, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), "990511/7896", "Jane", "Legue" },
                    { "1109F7061A", new DateTime(1989, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "890201/9896", "Lui", "Pale" },
                    { "8202FT8889", new DateTime(2001, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), "000812/6316", "John", "Paul" },
                    { "8802FT0989", new DateTime(2000, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "001212/6326", "Allen", "Rogue" },
                    { "9202AD0892", new DateTime(1969, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), "001003/6369", "Lys", "Lorence" },
                    { "9202FZ0912", new DateTime(1969, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "000102/0026", "Matheus", "Langus" }
                });

            migrationBuilder.InsertData(
                table: "Traffic",
                columns: new[] { "Id", "TrafficDevice" },
                values: new object[,]
                {
                    { 1, "Bus" },
                    { 2, "CompanyCar" },
                    { 3, "Plane" },
                    { 4, "PublicBus" },
                    { 5, "Taxi" },
                    { 6, "Train" },
                    { 7, "Walk" }
                });

            migrationBuilder.InsertData(
                table: "TravelOrder",
                columns: new[] { "Id", "EmployeeId", "EndDate", "EndPlaceCityId", "Note", "StartDate", "StartPlaceCityId", "State" },
                values: new object[,]
                {
                    { 1, "1109F7061A", new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "This travel order must be processed as soon as possible.", new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Created" },
                    { 2, "8202FT8889", new DateTime(2024, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, null, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Accounted" }
                });

            migrationBuilder.InsertData(
                table: "TrafficTravelOrder",
                columns: new[] { "TrafficsId", "TravelOrdersId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrafficTravelOrder_TravelOrdersId",
                table: "TrafficTravelOrder",
                column: "TravelOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelOrder_EmployeeId",
                table: "TravelOrder",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelOrder_EndPlaceCityId",
                table: "TravelOrder",
                column: "EndPlaceCityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelOrder_StartPlaceCityId",
                table: "TravelOrder",
                column: "StartPlaceCityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrafficTravelOrder");

            migrationBuilder.DropTable(
                name: "Traffic");

            migrationBuilder.DropTable(
                name: "TravelOrder");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FarmatikoData.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthFacilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Strength = table.Column<string>(nullable: false),
                    Form = table.Column<string>(nullable: true),
                    WayOfIssuing = table.Column<string>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Packaging = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pandemics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    TotalMK = table.Column<int>(nullable: false),
                    ActiveMK = table.Column<int>(nullable: false),
                    DeathsMK = table.Column<int>(nullable: false),
                    NewMK = table.Column<int>(nullable: false),
                    TotalGlobal = table.Column<long>(nullable: false),
                    DeathsGlobal = table.Column<long>(nullable: false),
                    ActiveGlobal = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pandemics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthcareWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Branch = table.Column<string>(nullable: false),
                    FacilityId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthcareWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthcareWorkers_HealthFacilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "HealthFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyHeads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyHeads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyHeads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    WorkAllTime = table.Column<bool>(nullable: false),
                    PheadId = table.Column<int>(nullable: false),
                    PharmacyHeadId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_PharmacyHeads_PharmacyHeadId",
                        column: x => x.PharmacyHeadId,
                        principalTable: "PharmacyHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyHeadMedicines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    PheadId = table.Column<int>(nullable: false),
                    HeadId = table.Column<int>(nullable: true),
                    MedicineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyHeadMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyHeadMedicines_PharmacyHeads_HeadId",
                        column: x => x.HeadId,
                        principalTable: "PharmacyHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmacyHeadMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PHRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "now()"),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    HeadId = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PHRequests_PharmacyHeads_HeadId",
                        column: x => x.HeadId,
                        principalTable: "PharmacyHeads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PHRequests_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthcareWorkers_FacilityId",
                table: "HealthcareWorkers",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PharmacyHeadId",
                table: "Pharmacies",
                column: "PharmacyHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyHeadMedicines_HeadId",
                table: "PharmacyHeadMedicines",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyHeadMedicines_MedicineId",
                table: "PharmacyHeadMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyHeads_UserId",
                table: "PharmacyHeads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PHRequests_HeadId",
                table: "PHRequests",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_PHRequests_PharmacyId",
                table: "PHRequests",
                column: "PharmacyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthcareWorkers");

            migrationBuilder.DropTable(
                name: "Pandemics");

            migrationBuilder.DropTable(
                name: "PharmacyHeadMedicines");

            migrationBuilder.DropTable(
                name: "PHRequests");

            migrationBuilder.DropTable(
                name: "HealthFacilities");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "PharmacyHeads");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

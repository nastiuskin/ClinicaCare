using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MedicalProcedureMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalProcedures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalProcedures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorMedicalProcedure",
                columns: table => new
                {
                    DoctorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    MedicalProceduresId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMedicalProcedure", x => new { x.DoctorsId, x.MedicalProceduresId });
                    table.ForeignKey(
                        name: "FK_DoctorMedicalProcedure_MedicalProcedures_MedicalProceduresId",
                        column: x => x.MedicalProceduresId,
                        principalTable: "MedicalProcedures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorMedicalProcedure_Users_DoctorsId",
                        column: x => x.DoctorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMedicalProcedure_MedicalProceduresId",
                table: "DoctorMedicalProcedure",
                column: "MedicalProceduresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorMedicalProcedure");

            migrationBuilder.DropTable(
                name: "MedicalProcedures");
        }
    }
}

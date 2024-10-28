using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicalProcedureAndUserTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Appointments",
                newName: "AppointmentDateTime");

            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"WorkingStartTime\" TYPE interval USING \"WorkingStartTime\" - '1970-01-01'::timestamp");

            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"WorkingEndTime\" TYPE interval USING \"WorkingEndTime\" - '1970-01-01'::timestamp");


            migrationBuilder.AlterColumn<TimeSpan>(
                name: "WorkingEndTime",
                table: "Users",
                type: "interval",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MedicalProcedures",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedures_Name",
                table: "MedicalProcedures",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalProcedures_Name",
                table: "MedicalProcedures");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MedicalProcedures");

            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Appointments",
                newName: "StartTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkingStartTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WorkingEndTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

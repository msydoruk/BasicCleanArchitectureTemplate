using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Web.Migrations
{
    public partial class AddFieldsToRecurrenceSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop 'Date' column from 'Events' table.
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Events");

            // Rename 'Time' column in 'Events' table to 'StartDate'.
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Events",
                newName: "StartDate");

            // Before altering 'Id', save data and remove FK constraints if necessary.

            // You need to drop and recreate the 'Id' column because of the IDENTITY property.
            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Events");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            // If there were any FKs, recreate them here using the new 'Id' column.

            // Create 'RecurrenceSetting' table.
            migrationBuilder.CreateTable(
                name: "RecurrenceSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    OccurrencePosition = table.Column<int>(type: "int", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    DayOfWeeks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurrenceSetting_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create index for 'EventId' in 'RecurrenceSetting' table to ensure uniqueness.
            migrationBuilder.CreateIndex(
                name: "IX_RecurrenceSetting_EventId",
                table: "RecurrenceSetting",
                column: "EventId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop 'RecurrenceSetting' table.
            migrationBuilder.DropTable(
                name: "RecurrenceSetting");

            // Rename 'StartDate' column in 'Events' table back to 'Time'.
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Events",
                newName: "Time");

            // Revert 'Id' column type from 'Guid' to 'int' in 'Events' table.
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");

            // Re-add 'Date' column to 'Events' table.
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

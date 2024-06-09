using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agile_dev.Migrations
{
    /// <inheritdoc />
    public partial class Added2FieldsToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCustomField_Event_EventId",
                table: "EventCustomField");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventCustomField",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AgeLimit",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Event",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCustomField_Event_EventId",
                table: "EventCustomField",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCustomField_Event_EventId",
                table: "EventCustomField");

            migrationBuilder.DropColumn(
                name: "AgeLimit",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventCustomField",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCustomField_Event_EventId",
                table: "EventCustomField",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

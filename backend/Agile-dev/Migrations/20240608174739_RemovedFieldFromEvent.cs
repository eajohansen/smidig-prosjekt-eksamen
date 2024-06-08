using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agile_dev.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFieldFromEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomField_Event_EventId",
                table: "CustomField");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCustomField_CustomField_CustomFieldId",
                table: "EventCustomField");

            migrationBuilder.DropIndex(
                name: "IX_CustomField_EventId",
                table: "CustomField");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "CustomField");

            migrationBuilder.AlterColumn<int>(
                name: "CustomFieldId",
                table: "EventCustomField",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCustomField_CustomField_CustomFieldId",
                table: "EventCustomField",
                column: "CustomFieldId",
                principalTable: "CustomField",
                principalColumn: "CustomFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCustomField_CustomField_CustomFieldId",
                table: "EventCustomField");

            migrationBuilder.AlterColumn<int>(
                name: "CustomFieldId",
                table: "EventCustomField",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "CustomField",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomField_EventId",
                table: "CustomField",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomField_Event_EventId",
                table: "CustomField",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCustomField_CustomField_CustomFieldId",
                table: "EventCustomField",
                column: "CustomFieldId",
                principalTable: "CustomField",
                principalColumn: "CustomFieldId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

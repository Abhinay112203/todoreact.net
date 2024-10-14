using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    public partial class AddStageCodeUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ToDoItems");

            migrationBuilder.AddColumn<string>(
                name: "StageId",
                table: "ToDoItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_StageId",
                table: "ToDoItems",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_Stages_StageId",
                table: "ToDoItems",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_Stages_StageId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_StageId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stages");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ToDoItems",
                type: "TEXT",
                nullable: true);
        }
    }
}

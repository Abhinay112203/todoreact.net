using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    public partial class ToDoItemFkeyList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListId",
                table: "ToDoItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_ListId",
                table: "ToDoItems",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_Lists_ListId",
                table: "ToDoItems",
                column: "ListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_Lists_ListId",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_ListId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "ToDoItems");
        }
    }
}

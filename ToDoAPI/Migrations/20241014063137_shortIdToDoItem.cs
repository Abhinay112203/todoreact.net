using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    public partial class shortIdToDoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "shortId",
                table: "ToDoItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shortId",
                table: "ToDoItems");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoAPI.Migrations
{
    public partial class UpdatedDeleteUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Deleted",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }
    }
}

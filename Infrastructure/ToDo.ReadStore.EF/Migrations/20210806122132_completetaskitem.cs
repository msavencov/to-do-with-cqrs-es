using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Store.Migrations
{
    public partial class completetaskitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedTaskCount",
                table: "ToDoListReadModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ToDoItemReadModel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedTaskCount",
                table: "ToDoListReadModel");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ToDoItemReadModel");
        }
    }
}

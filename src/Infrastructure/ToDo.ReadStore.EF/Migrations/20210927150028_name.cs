using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Store.Migrations
{
    public partial class name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Task",
                table: "ToDoItemReadModel",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ToDoItemReadModel",
                newName: "Task");
        }
    }
}

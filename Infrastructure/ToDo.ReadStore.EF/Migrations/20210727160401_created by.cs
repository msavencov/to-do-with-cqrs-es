using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Store.Migrations
{
    public partial class createdby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ToDoListReadModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ToDoItemReadModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ToDoListReadModel");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ToDoItemReadModel");
        }
    }
}

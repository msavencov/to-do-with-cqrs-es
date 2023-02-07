using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Store.Migrations
{
    public partial class deleteitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ToDoItemReadModel",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ToDoItemReadModel");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Store.Migrations
{
    public partial class todomodelrefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItemModel");

            migrationBuilder.DropTable(
                name: "ToDoReadModel");

            migrationBuilder.CreateTable(
                name: "ToDoItemReadModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItemReadModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoListReadModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoListReadModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoItemReadModel");

            migrationBuilder.DropTable(
                name: "ToDoListReadModel");

            migrationBuilder.CreateTable(
                name: "ToDoReadModel",
                columns: table => new
                {
                    ListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoReadModel", x => x.ListId);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItemModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToDoReadModelListId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItemModel_ToDoReadModel_ToDoReadModelListId",
                        column: x => x.ToDoReadModelListId,
                        principalTable: "ToDoReadModel",
                        principalColumn: "ListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItemModel_ToDoReadModelListId",
                table: "ToDoItemModel",
                column: "ToDoReadModelListId");
        }
    }
}

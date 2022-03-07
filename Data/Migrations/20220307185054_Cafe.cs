using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Cafe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cafe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Produtor = table.Column<string>(nullable: false),
                    NomeCafe = table.Column<string>(nullable: false),
                    Nota = table.Column<int>(nullable: false),
                    Regiao = table.Column<string>(nullable: false),
                    Impressoes = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cafe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CafeComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<DateTime>(nullable: false),
                    CafesId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CafeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CafeComments_Cafe_CafesId",
                        column: x => x.CafesId,
                        principalTable: "Cafe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CafeComments_CafesId",
                table: "CafeComments",
                column: "CafesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CafeComments");

            migrationBuilder.DropTable(
                name: "Cafe");
        }
    }
}

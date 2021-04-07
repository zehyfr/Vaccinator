using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vaccinator.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personnes",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "TEXT", nullable: false),
                    nom = table.Column<string>(type: "TEXT", nullable: false),
                    prenom = table.Column<string>(type: "TEXT", nullable: false),
                    sexe = table.Column<char>(type: "TEXT", nullable: false),
                    ddn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "Injections",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "TEXT", nullable: false),
                    Personneuuid = table.Column<string>(type: "TEXT", nullable: false),
                    Maladie = table.Column<string>(type: "TEXT", nullable: false),
                    Marque = table.Column<string>(type: "TEXT", nullable: false),
                    NumLot = table.Column<string>(type: "TEXT", nullable: false),
                    DatePrise = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRappel = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusRappel = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Injections", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_Injections_Personnes_Personneuuid",
                        column: x => x.Personneuuid,
                        principalTable: "Personnes",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Injections_Personneuuid",
                table: "Injections",
                column: "Personneuuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Injections");

            migrationBuilder.DropTable(
                name: "Personnes");
        }
    }
}

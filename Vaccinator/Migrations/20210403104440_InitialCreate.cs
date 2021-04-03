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
                    uuid = table.Column<string>(type: "text", nullable: false),
                    nom = table.Column<string>(type: "text", nullable: false),
                    prenom = table.Column<string>(type: "text", nullable: false),
                    sexe = table.Column<char>(type: "character(1)", nullable: false),
                    ddn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "Injections",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "text", nullable: false),
                    Maladie = table.Column<string>(type: "text", nullable: false),
                    Marque = table.Column<string>(type: "text", nullable: false),
                    NumLot = table.Column<string>(type: "text", nullable: false),
                    DatePrise = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateRappel = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StatusRappel = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Injections", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_Injections_Personnes_uuid",
                        column: x => x.uuid,
                        principalTable: "Personnes",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });
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

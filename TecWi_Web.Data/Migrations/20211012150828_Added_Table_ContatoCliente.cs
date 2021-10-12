using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Added_Table_ContatoCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClienteContato",
                columns: table => new
                {
                    IdClienteContato = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cdclifor = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteContato", x => x.IdClienteContato);
                    table.ForeignKey(
                        name: "FK_ClienteContato_Cliente_Cdclifor",
                        column: x => x.Cdclifor,
                        principalTable: "Cliente",
                        principalColumn: "Cdclifor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteContato_Cdclifor",
                table: "ClienteContato",
                column: "Cdclifor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteContato");
        }
    }
}

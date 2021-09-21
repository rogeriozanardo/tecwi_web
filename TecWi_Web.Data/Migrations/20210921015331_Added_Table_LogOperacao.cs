using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Added_Table_LogOperacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogOperacao",
                columns: table => new
                {
                    IdLogOperacao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoOperacao = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogOperacao", x => x.IdLogOperacao);
                    table.ForeignKey(
                        name: "FK_LogOperacao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogOperacao_IdUsuario",
                table: "LogOperacao",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogOperacao");
        }
    }
}

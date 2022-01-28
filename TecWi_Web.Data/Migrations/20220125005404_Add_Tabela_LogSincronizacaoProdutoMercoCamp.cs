using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Add_Tabela_LogSincronizacaoProdutoMercoCamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogSincronizacaoProdutoMercoCamp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InicioSincronizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodoInicialEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodoFinalEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JsonEnvio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSincronizacaoProdutoMercoCamp", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogSincronizacaoProdutoMercoCamp");
        }
    }
}

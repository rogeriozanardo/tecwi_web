using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Add_Tabelas_MovimentoFiscal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimentoFiscal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumMovimento = table.Column<int>(type: "int", nullable: false),
                    CdFilial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroNota = table.Column<int>(type: "int", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: false),
                    QtdVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChaveAcesso = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoFiscal", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MovimentoFiscalItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMovimentoFiscal = table.Column<int>(type: "int", nullable: false),
                    Sequencia = table.Column<int>(type: "int", nullable: false),
                    CdProduto = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Qtd = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoFiscalItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MovimentoFiscalItem_MovimentoFiscal_IDMovimentoFiscal",
                        column: x => x.IDMovimentoFiscal,
                        principalTable: "MovimentoFiscal",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoFiscalItem_IDMovimentoFiscal",
                table: "MovimentoFiscalItem",
                column: "IDMovimentoFiscal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentoFiscalItem");

            migrationBuilder.DropTable(
                name: "MovimentoFiscal");
        }
    }
}

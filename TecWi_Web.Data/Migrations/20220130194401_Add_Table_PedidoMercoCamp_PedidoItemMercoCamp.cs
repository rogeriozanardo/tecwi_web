using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Add_Table_PedidoMercoCamp_PedidoItemMercoCamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoMercoCamp",
                columns: table => new
                {
                    IdPedidoMercoCamp = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumPedido = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(19,10)", precision: 19, scale: 10, nullable: false),
                    SeqTransmissao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoMercoCamp", x => x.IdPedidoMercoCamp);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItemMercoCamp",
                columns: table => new
                {
                    IdPedidoItem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoMercoCampId = table.Column<int>(type: "int", nullable: false),
                    NumPedido = table.Column<int>(type: "int", nullable: false),
                    SeqTransmissao = table.Column<int>(type: "int", nullable: false),
                    Qtd = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: false),
                    CdProduto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QtdSeparado = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItemMercoCamp", x => x.IdPedidoItem);
                    table.ForeignKey(
                        name: "FK_PedidoItemMercoCamp_PedidoMercoCamp_PedidoMercoCampId",
                        column: x => x.PedidoMercoCampId,
                        principalTable: "PedidoMercoCamp",
                        principalColumn: "IdPedidoMercoCamp");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItemMercoCamp_PedidoMercoCampId",
                table: "PedidoItemMercoCamp",
                column: "PedidoMercoCampId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItemMercoCamp");

            migrationBuilder.DropTable(
                name: "PedidoMercoCamp");
        }
    }
}

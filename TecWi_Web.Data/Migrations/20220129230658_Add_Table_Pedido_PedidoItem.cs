using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Add_Table_Pedido_PedidoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cdempresa = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    cdfilial = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    cdcliente = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    nummovimento = table.Column<int>(type: "int", nullable: false),
                    nummovcliente = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    dtinicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    stpendencia = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    cdvendedor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    stpagafrete = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    cdtransportadora = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    cdtransportadoraredespacho = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    stpagafreteredespacho = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPedido = table.Column<int>(type: "int", nullable: false),
                    cdempresa = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    cdfilial = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    nummovimento = table.Column<int>(type: "int", nullable: false),
                    seq = table.Column<int>(type: "int", nullable: false),
                    cdproduto = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    tpregistro = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    qtdsolicitada = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: true),
                    qtdprocessada = table.Column<decimal>(type: "decimal(19,8)", precision: 19, scale: 8, nullable: true),
                    VlVenda = table.Column<decimal>(type: "decimal(19,10)", precision: 19, scale: 10, nullable: true),
                    vlcalculado = table.Column<decimal>(type: "decimal(19,10)", precision: 19, scale: 10, nullable: true),
                    uporigem = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedido_IDPedido",
                        column: x => x.IDPedido,
                        principalTable: "Pedido",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_IDPedido",
                table: "PedidoItem",
                column: "IDPedido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "Pedido");
        }
    }
}

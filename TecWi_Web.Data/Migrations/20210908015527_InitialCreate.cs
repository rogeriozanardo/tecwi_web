using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SenhaHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Cdclifor = table.Column<int>(type: "int", nullable: false),
                    Inscrifed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fantasia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Razao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DDD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fone1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fone2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Cdclifor);
                    table.ForeignKey(
                        name: "FK_Cliente_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioAplicacao",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAplicacao = table.Column<int>(type: "int", nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioAplicacao", x => new { x.IdUsuario, x.IdAplicacao, x.IdPerfil });
                    table.ForeignKey(
                        name: "FK_UsuarioAplicacao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "PagarReceber",
                columns: table => new
                {
                    Numlancto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sq = table.Column<int>(type: "int", nullable: false),
                    Cdfilial = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeqID = table.Column<int>(type: "int", nullable: false),
                    Stcobranca = table.Column<bool>(type: "bit", nullable: false),
                    Dtemissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dtvencto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dtpagto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Valorr = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NotasFiscais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cdclifor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagarReceber", x => new { x.Numlancto, x.Sq, x.Cdfilial });
                    table.ForeignKey(
                        name: "FK_PagarReceber_Cliente_Cdclifor",
                        column: x => x.Cdclifor,
                        principalTable: "Cliente",
                        principalColumn: "Cdclifor");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_IdUsuario",
                table: "Cliente",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PagarReceber_Cdclifor",
                table: "PagarReceber",
                column: "Cdclifor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagarReceber");

            migrationBuilder.DropTable(
                name: "UsuarioAplicacao");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Add_Tabelas_Transportadora_Empresa_Vendedor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    CdCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Razao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Fantasia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StAtivo = table.Column<int>(type: "int", nullable: false),
                    UpdRegistro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cep = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Complemento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DDD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Fone1 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Fone2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UF = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    InscriEst = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InscriFed = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.EmpresaId);
                });

            migrationBuilder.CreateTable(
                name: "Transportadora",
                columns: table => new
                {
                    IdTransportadora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CdTransportadora = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Inscrifed = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    Tpinscricao = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Fantasia = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UpdRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StAtivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportadora", x => x.IdTransportadora);
                });

            migrationBuilder.CreateTable(
                name: "Vendedor",
                columns: table => new
                {
                    IdVendedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CdVendedor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    UpdRegistro = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedor", x => x.IdVendedor);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Transportadora");

            migrationBuilder.DropTable(
                name: "Vendedor");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TecWi_Web.Data.Migrations
{
    public partial class Alter_Table_MovimentoFiscal_Precision_Qtd_Volume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QtdVolume",
                table: "MovimentoFiscal",
                type: "decimal(19,8)",
                precision: 19,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QtdVolume",
                table: "MovimentoFiscal",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,8)",
                oldPrecision: 19,
                oldScale: 8);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliza.Migrations
{
    /// <inheritdoc />
    public partial class _202505150013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Prima",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DECIMAL(18,2)",
                nullable: true,
                comment: "Prima de la póliza",
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)",
                oldComment: "Prima de la póliza");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoAsegurado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DECIMAL(18,2)",
                nullable: true,
                comment: "Monto asegurado",
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)",
                oldComment: "Monto asegurado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Prima",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Prima de la póliza",
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)",
                oldNullable: true,
                oldComment: "Prima de la póliza");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontoAsegurado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Monto asegurado",
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)",
                oldNullable: true,
                oldComment: "Monto asegurado");
        }
    }
}

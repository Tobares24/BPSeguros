using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliza.Migrations
{
    /// <inheritdoc />
    public partial class _202505150047 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroPoliza",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR(128)",
                maxLength: 128,
                nullable: true,
                comment: "Número de póliza",
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "Número de póliza");

            migrationBuilder.AlterColumn<string>(
                name: "CedulaAsegurado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true,
                comment: "Cédula del asegurado",
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldNullable: true,
                oldComment: "Cédula del asegurado");

            migrationBuilder.AlterColumn<string>(
                name: "Aseguradora",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR(128)",
                maxLength: 128,
                nullable: true,
                comment: "Nombre de la aseguradora",
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldMaxLength: 150,
                oldNullable: true,
                oldComment: "Nombre de la aseguradora");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumeroPoliza",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true,
                comment: "Número de póliza",
                oldClrType: typeof(string),
                oldType: "VARCHAR(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "Número de póliza");

            migrationBuilder.AlterColumn<string>(
                name: "CedulaAsegurado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR",
                nullable: true,
                comment: "Cédula del asegurado",
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "Cédula del asegurado");

            migrationBuilder.AlterColumn<string>(
                name: "Aseguradora",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: true,
                comment: "Nombre de la aseguradora",
                oldClrType: typeof(string),
                oldType: "VARCHAR(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "Nombre de la aseguradora");
        }
    }
}

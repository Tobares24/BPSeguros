using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliza.Migrations
{
    /// <inheritdoc />
    public partial class _202505150016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Periodo",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: true,
                comment: "Periodo de cobertura de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldComment: "Periodo de cobertura de la póliza");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInclusion",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: true,
                comment: "Fecha de inclusión de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldComment: "Fecha de inclusión de la póliza");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEmision",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: true,
                comment: "Fecha de emisión de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldComment: "Fecha de emisión de la póliza");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Periodo",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Periodo de cobertura de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "Periodo de cobertura de la póliza");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInclusion",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Fecha de inclusión de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "Fecha de inclusión de la póliza");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEmision",
                schema: "PolizaSchema",
                table: "PolizaTable",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Fecha de emisión de la póliza",
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true,
                oldComment: "Fecha de emisión de la póliza");
        }
    }
}

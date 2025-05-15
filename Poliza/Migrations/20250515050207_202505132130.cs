using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliza.Migrations
{
    /// <inheritdoc />
    public partial class _202505132130 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PolizaSchema");

            migrationBuilder.CreateTable(
                name: "PolizaCoberturaTable",
                schema: "PolizaSchema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del tipo de póliza"),
                    Descripcion = table.Column<string>(type: "VARCHAR(128)", maxLength: 128, nullable: true, comment: "Descripción de la cobertura"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolizaCobertura_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolizaEstadoTable",
                schema: "PolizaSchema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del estado de la póliza"),
                    Descripcion = table.Column<string>(type: "VARCHAR(128)", maxLength: 128, nullable: true, comment: "Descripción del estado de la póliza"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolizaEstado_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoPoliza",
                schema: "PolizaSchema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del tipo de póliza"),
                    Descripcion = table.Column<string>(type: "VARCHAR(512)", maxLength: 512, nullable: true, comment: "Descripción del tipo de póliza"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPoliza", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolizaTable",
                schema: "PolizaSchema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador de la póliza"),
                    NumeroPoliza = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true, comment: "Número de póliza"),
                    IdTipoPoliza = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del tipo de póliza"),
                    CedulaAsegurado = table.Column<string>(type: "VARCHAR", nullable: true, comment: "Cédula del asegurado"),
                    MontoAsegurado = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "Monto asegurado"),
                    FechaVencimiento = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Fecha de vencimiento de la póliza"),
                    FechaEmision = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Fecha de emisión de la póliza"),
                    IdCobertura = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true, comment: "Coberturas de la póliza"),
                    IdPolizaEstado = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del estado de la póliza"),
                    Prima = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "Prima de la póliza"),
                    Periodo = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Periodo de cobertura de la póliza"),
                    FechaInclusion = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Fecha de inclusión de la póliza"),
                    Aseguradora = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: true, comment: "Nombre de la aseguradora"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poliza_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poliza_PolizaCobertura",
                        column: x => x.IdCobertura,
                        principalSchema: "PolizaSchema",
                        principalTable: "PolizaCoberturaTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Poliza_PolizaEstado",
                        column: x => x.IdPolizaEstado,
                        principalSchema: "PolizaSchema",
                        principalTable: "PolizaEstadoTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Poliza_TipoPoliza",
                        column: x => x.IdTipoPoliza,
                        principalSchema: "PolizaSchema",
                        principalTable: "TipoPoliza",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "TipoPolizaBusquedaIndex",
                schema: "PolizaSchema",
                table: "PolizaCoberturaTable",
                columns: new[] { "Descripcion", "EstaEliminado" },
                unique: true,
                filter: "[Descripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "TipoPolizaIndex",
                schema: "PolizaSchema",
                table: "PolizaCoberturaTable",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PolizaEstadoBusquedaIndex",
                schema: "PolizaSchema",
                table: "PolizaEstadoTable",
                columns: new[] { "Descripcion", "EstaEliminado" },
                unique: true,
                filter: "[Descripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "PolizaEstadoIndex",
                schema: "PolizaSchema",
                table: "PolizaEstadoTable",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_IdCobertura",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdCobertura");

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_IdPolizaEstado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdPolizaEstado");

            migrationBuilder.CreateIndex(
                name: "PolizaCedulaAseguradoIndex",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "CedulaAsegurado");

            migrationBuilder.CreateIndex(
                name: "PolizaFechaVencimientoIndex",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "PolizaIdTipoPolizaIndex",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdTipoPoliza");

            migrationBuilder.CreateIndex(
                name: "PolizaNumeroPolizaIndex",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "NumeroPoliza");

            migrationBuilder.CreateIndex(
                name: "TipoPolizaBusquedaIndex",
                schema: "PolizaSchema",
                table: "TipoPoliza",
                columns: new[] { "Descripcion", "EstaEliminado" },
                unique: true,
                filter: "[Descripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "TipoPolizaIndex",
                schema: "PolizaSchema",
                table: "TipoPoliza",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolizaTable",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "PolizaCoberturaTable",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "PolizaEstadoTable",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "TipoPoliza",
                schema: "PolizaSchema");
        }
    }
}

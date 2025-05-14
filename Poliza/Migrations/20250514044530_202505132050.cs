using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poliza.Migrations
{
    /// <inheritdoc />
    public partial class _202505132050 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Persona");

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
                name: "TipoPersonaTable",
                schema: "Persona",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del catálogo de tipo de persona"),
                    TipoPersona = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true, comment: "Tipo de persona"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPersonaTable", x => x.Id);
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
                name: "PersonaTable",
                schema: "Persona",
                columns: table => new
                {
                    CedulaAsegurado = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: false, comment: "Cédula del asegurado de la persona"),
                    Nombre = table.Column<string>(type: "VARCHAR(512)", maxLength: 512, nullable: true, comment: "Nombre de la persona"),
                    PrimerApellido = table.Column<string>(type: "VARCHAR(128)", maxLength: 128, nullable: true, comment: "Primer apellido de la persona"),
                    SegundoApellido = table.Column<string>(type: "VARCHAR(128)", maxLength: 128, nullable: true, comment: "Segundo apellido de la persona"),
                    IdTipoPersona = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador de la persona con la que se relaciona la persona"),
                    FechaNacimiento = table.Column<DateTime>(type: "DATETIME", nullable: true, comment: "Fecha de nacimiento de la persona"),
                    EstaEliminado = table.Column<bool>(type: "BIT", nullable: false, comment: "Indicador de borrado lógico")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaTable", x => x.CedulaAsegurado);
                    table.ForeignKey(
                        name: "FK_PersonaTable_TipoPersonaTable_IdTipoPersona",
                        column: x => x.IdTipoPersona,
                        principalSchema: "Persona",
                        principalTable: "TipoPersonaTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PolizaTable",
                schema: "PolizaSchema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador de la póliza"),
                    NumeroPoliza = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true, comment: "Número de póliza"),
                    IdTipoPoliza = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Identificador del tipo de póliza"),
                    CedulaAsegurado = table.Column<string>(type: "VARCHAR(64)", nullable: true, comment: "Cédula del asegurado"),
                    MontoAsegurado = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false, comment: "Monto asegurado"),
                    FechaVencimiento = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Fecha de vencimiento de la póliza"),
                    FechaEmision = table.Column<DateTime>(type: "DATETIME", nullable: false, comment: "Fecha de emisión de la póliza"),
                    IdCobertura = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, comment: "Coberturas de la póliza"),
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
                        name: "FK_Poliza_Persona",
                        column: x => x.CedulaAsegurado,
                        principalSchema: "Persona",
                        principalTable: "PersonaTable",
                        principalColumn: "CedulaAsegurado");
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
                name: "EstaEliminadoIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "EstaEliminado");

            migrationBuilder.CreateIndex(
                name: "IdTipoPersonaIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "IdTipoPersona");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaTable_IdTipoPersona",
                schema: "Persona",
                table: "PersonaTable",
                column: "IdTipoPersona",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NombreIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "PersonaIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "CedulaAsegurado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "PrimerApellidoIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "PrimerApellido");

            migrationBuilder.CreateIndex(
                name: "SegundoApellidoIndex",
                schema: "Persona",
                table: "PersonaTable",
                column: "SegundoApellido");

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
                name: "TipoPolizaBusquedaIndex",
                schema: "PolizaSchema",
                table: "PolizaEstadoTable",
                columns: new[] { "Descripcion", "EstaEliminado" },
                unique: true,
                filter: "[Descripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "TipoPolizaIndex",
                schema: "PolizaSchema",
                table: "PolizaEstadoTable",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_CedulaAsegurado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "CedulaAsegurado",
                unique: true,
                filter: "[CedulaAsegurado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_IdCobertura",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdCobertura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_IdPolizaEstado",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdPolizaEstado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolizaTable_IdTipoPoliza",
                schema: "PolizaSchema",
                table: "PolizaTable",
                column: "IdTipoPoliza",
                unique: true);

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
                name: "TipoPersonaIndex",
                schema: "Persona",
                table: "TipoPersonaTable",
                column: "Id",
                unique: true);

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
                name: "PersonaTable",
                schema: "Persona");

            migrationBuilder.DropTable(
                name: "PolizaCoberturaTable",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "PolizaEstadoTable",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "TipoPoliza",
                schema: "PolizaSchema");

            migrationBuilder.DropTable(
                name: "TipoPersonaTable",
                schema: "Persona");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persona.Migrations
{
    /// <inheritdoc />
    public partial class _202505132110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Persona");

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
                    table.PrimaryKey("PK_TipoPersona_Id", x => x.Id);
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
                    table.PrimaryKey("PK_Persona_CedulaAsegurado", x => x.CedulaAsegurado);
                    table.ForeignKey(
                        name: "FK_Persona_TipoPersona",
                        column: x => x.IdTipoPersona,
                        principalSchema: "Persona",
                        principalTable: "TipoPersonaTable",
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
                name: "TipoPersonaIndex",
                schema: "Persona",
                table: "TipoPersonaTable",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonaTable",
                schema: "Persona");

            migrationBuilder.DropTable(
                name: "TipoPersonaTable",
                schema: "Persona");
        }
    }
}

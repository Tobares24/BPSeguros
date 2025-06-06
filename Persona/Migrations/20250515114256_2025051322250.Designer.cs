﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persona.Entities;

#nullable disable

namespace Persona.Migrations
{
    [DbContext(typeof(PersonaDbContext))]
    [Migration("20250515114256_2025051322250")]
    partial class _2025051322250
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Common.Entities.PersonaEntity", b =>
                {
                    b.Property<string>("CedulaAsegurado")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR")
                        .HasColumnOrder(1)
                        .HasComment("Cédula del asegurado de la persona");

                    b.Property<bool>("EstaEliminado")
                        .HasColumnType("BIT")
                        .HasColumnOrder(7)
                        .HasComment("Indicador de borrado lógico");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("DATETIME")
                        .HasColumnOrder(6)
                        .HasComment("Fecha de nacimiento de la persona");

                    b.Property<Guid>("IdTipoPersona")
                        .HasColumnType("UNIQUEIDENTIFIER")
                        .HasColumnOrder(5)
                        .HasComment("Identificador de la persona con la que se relaciona la persona");

                    b.Property<string>("Nombre")
                        .HasMaxLength(512)
                        .HasColumnType("VARCHAR")
                        .HasColumnOrder(2)
                        .HasComment("Nombre de la persona");

                    b.Property<string>("PrimerApellido")
                        .HasMaxLength(128)
                        .HasColumnType("VARCHAR")
                        .HasColumnOrder(3)
                        .HasComment("Primer apellido de la persona");

                    b.Property<string>("SegundoApellido")
                        .HasMaxLength(128)
                        .HasColumnType("VARCHAR")
                        .HasColumnOrder(4)
                        .HasComment("Segundo apellido de la persona");

                    b.HasKey("CedulaAsegurado")
                        .HasName("PK_Persona_CedulaAsegurado");

                    b.HasIndex(new[] { "EstaEliminado" }, "EstaEliminadoIndex");

                    b.HasIndex(new[] { "IdTipoPersona" }, "IdTipoPersonaIndex");

                    b.HasIndex(new[] { "Nombre" }, "NombreIndex");

                    b.HasIndex(new[] { "CedulaAsegurado" }, "PersonaIndex")
                        .IsUnique();

                    b.HasIndex(new[] { "PrimerApellido" }, "PrimerApellidoIndex");

                    b.HasIndex(new[] { "SegundoApellido" }, "SegundoApellidoIndex");

                    b.ToTable("PersonaTable", "PersonaSchema");
                });

            modelBuilder.Entity("Common.Entities.TipoPersonaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UNIQUEIDENTIFIER")
                        .HasColumnOrder(1)
                        .HasComment("Identificador del catálogo de tipo de persona");

                    b.Property<bool>("EstaEliminado")
                        .HasColumnType("BIT")
                        .HasColumnOrder(3)
                        .HasComment("Indicador de borrado lógico");

                    b.Property<string>("TipoPersona")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR")
                        .HasColumnOrder(2)
                        .HasComment("Tipo de persona");

                    b.HasKey("Id")
                        .HasName("PK_TipoPersona_Id");

                    b.HasIndex(new[] { "TipoPersona" }, "TipoPersonaBusquedaIndex");

                    b.ToTable("TipoPersonaTable", "PersonaSchema");
                });

            modelBuilder.Entity("Common.Entities.PersonaEntity", b =>
                {
                    b.HasOne("Common.Entities.TipoPersonaEntity", "TipoPersona")
                        .WithMany("Personas")
                        .HasForeignKey("IdTipoPersona")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Persona_TipoPersona");

                    b.Navigation("TipoPersona");
                });

            modelBuilder.Entity("Common.Entities.TipoPersonaEntity", b =>
                {
                    b.Navigation("Personas");
                });
#pragma warning restore 612, 618
        }
    }
}

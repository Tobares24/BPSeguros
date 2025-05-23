﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("PolizaTable", Schema = "PolizaSchema")]
    [Index(nameof(NumeroPoliza), IsUnique = false, Name = "PolizaNumeroPolizaIndex")]
    [Index(nameof(IdTipoPoliza), IsUnique = false, Name = "PolizaIdTipoPolizaIndex")]
    [Index(nameof(CedulaAsegurado), IsUnique = false, Name = "PolizaCedulaAseguradoIndex")]
    [Index(nameof(FechaVencimiento), IsUnique = false, Name = "PolizaFechaVencimientoIndex")]
    public class PolizaEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador de la póliza")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "La cédula del asegurado debe tener un máximo de 128 caracteres y un mínimo de 1 caracter")]
        [Comment("Número de póliza")]
        public string? NumeroPoliza { get; set; }

        [Column(Order = 3, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del tipo de póliza")]
        public Guid IdTipoPoliza { get; set; }

        [ForeignKey(nameof(IdTipoPoliza))]
        public virtual TipoPolizaEntity? TipoPoliza { get; set; }

        [Column(Order = 4, TypeName = "VARCHAR")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "La cédula del asegurado debe tener un máximo de 64 caracteres y un mínimo de 1 caracter")]
        [Comment("Cédula del asegurado")]
        public string? CedulaAsegurado { get; set; }

        [NotMapped]
        [ForeignKey(nameof(CedulaAsegurado))]
        public virtual PersonaEntity? Persona { get; set; }

        [Column(Order = 5, TypeName = "DECIMAL(18, 2)")]
        [Comment("Monto asegurado")]
        public decimal? MontoAsegurado { get; set; }

        [Column(Order = 6, TypeName = "DATETIME")]
        [Comment("Fecha de vencimiento de la póliza")]
        public DateTime FechaVencimiento { get; set; }

        [Column(Order = 7, TypeName = "DATETIME")]
        [Comment("Fecha de emisión de la póliza")]
        public DateTime? FechaEmision { get; set; }

        [Column(Order = 8, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Coberturas de la póliza")]
        public Guid? IdCobertura { get; set; }

        [ForeignKey(nameof(IdCobertura))]
        public virtual PolizaCoberturaEntity? PolizaCobertura { get; set; }

        [Column(Order = 9, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del estado de la póliza")]
        public Guid IdPolizaEstado { get; set; }

        [ForeignKey(nameof(IdPolizaEstado))]
        public virtual PolizaEstadoEntity? PolizaEstado { get; set; }

        [Column(Order = 10, TypeName = "DECIMAL(18, 2)")]
        [Comment("Prima de la póliza")]
        public decimal? Prima { get; set; }

        [Column(Order = 11, TypeName = "DATETIME")]
        [Comment("Periodo de cobertura de la póliza")]
        public DateTime? Periodo { get; set; }

        [Column(Order = 12, TypeName = "DATETIME")]
        [Comment("Fecha de inclusión de la póliza")]
        public DateTime? FechaInclusion { get; set; }

        [Column(Order = 13, TypeName = "VARCHAR")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "La aseguradora debe tener un máximo de 128 caracteres y un mínimo de 1 caracter")]
        [Comment("Nombre de la aseguradora")]
        public string? Aseguradora { get; set; }

        [Column(Order = 14, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }
    }
}
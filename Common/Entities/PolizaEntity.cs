using Microsoft.EntityFrameworkCore;
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
        [StringLength(64)]
        [Comment("Número de póliza")]
        public string? NumeroPoliza { get; set; }

        [Column(Order = 3, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del tipo de póliza")]
        public Guid IdTipoPoliza { get; set; }

        [ForeignKey(nameof(IdTipoPoliza))]
        public virtual TipoPolizaEntity? TipoPoliza { get; set; }

        [Column(Order = 4, TypeName = "VARCHAR")]
        [Comment("Cédula del asegurado")]
        public string? CedulaAsegurado { get; set; }

        [NotMapped]
        [ForeignKey(nameof(CedulaAsegurado))]
        public virtual PersonaEntity? Persona { get; set; }

        [Column(Order = 5, TypeName = "DECIMAL(18, 2)")]
        [Comment("Monto asegurado")]
        public decimal MontoAsegurado { get; set; }

        [Column(Order = 6, TypeName = "DATETIME")]
        [Comment("Fecha de vencimiento de la póliza")]
        public DateTime FechaVencimiento { get; set; }

        [Column(Order = 7, TypeName = "DATETIME")]
        [Comment("Fecha de emisión de la póliza")]
        public DateTime FechaEmision { get; set; }

        [Column(Order = 8, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Coberturas de la póliza")]
        public Guid IdCobertura { get; set; }

        [ForeignKey(nameof(IdCobertura))]
        public virtual PolizaCoberturaEntity? PolizaCobertura { get; set; }

        [Column(Order = 9, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del estado de la póliza")]
        public Guid IdPolizaEstado { get; set; }

        [ForeignKey(nameof(IdPolizaEstado))]
        public virtual PolizaEstadoEntity? PolizaEstado { get; set; }

        [Column(Order = 10, TypeName = "DECIMAL(18, 2)")]
        [Comment("Prima de la póliza")]
        public decimal Prima { get; set; }

        [Column(Order = 11, TypeName = "DATETIME")]
        [Comment("Periodo de cobertura de la póliza")]
        public DateTime Periodo { get; set; }

        [Column(Order = 12, TypeName = "DATETIME")]
        [Comment("Fecha de inclusión de la póliza")]
        public DateTime FechaInclusion { get; set; }

        [Column(Order = 13, TypeName = "VARCHAR")]
        [StringLength(150)]
        [Comment("Nombre de la aseguradora")]
        public string? Aseguradora { get; set; }

        [Column(Order = 14, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Periodo de la póliza")]
        public Guid IdPeriodo { get; set; }

        [ForeignKey(nameof(IdPeriodo))]
        public virtual PolizaPeriodoEntity? PolizaPeriodo { get; set; }

        [Column(Order = 15, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }
    }
}
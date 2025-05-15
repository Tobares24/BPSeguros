using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("PolizaPeriodo", Schema = "PolizaSchema")]
    [Index(nameof(Id), IsUnique = true, Name = "PolizaPeriodoIndex")]
    [Index(nameof(Descripcion), nameof(EstaEliminado), IsUnique = true, Name = "PolizaPeriodoBusquedaIndex")]
    public class PolizaPeriodoEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del periodo de póliza")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(128)]
        [Comment("Descripción del periodo de póliza")]
        public string? Descripcion { get; set; }

        [Column(Order = 3, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }

        public virtual List<PolizaEntity>? Polizas { get; set; }
    }
}

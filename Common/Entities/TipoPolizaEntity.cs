using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("TipoPoliza", Schema = "PolizaSchema")]
    [Index(nameof(Id), IsUnique = true, Name = "TipoPolizaIndex")]
    [Index(nameof(Descripcion), nameof(EstaEliminado), IsUnique = true, Name = "TipoPolizaBusquedaIndex")]
    public class TipoPolizaEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del tipo de póliza")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(512)]
        [Comment("Descripción del tipo de póliza")]
        public string? Descripcion { get; set; }

        [Column(Order = 3, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }

        public virtual List<PolizaEntity>? Polizas { get; set; }
    }
}

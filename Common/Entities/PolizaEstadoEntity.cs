using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("PolizaEstadoTable", Schema = "PolizaSchema")]
    [Index(nameof(Id), IsUnique = true, Name = "TipoPolizaIndex")]
    [Index(nameof(Descripcion), nameof(EstaEliminado), IsUnique = true, Name = "TipoPolizaBusquedaIndex")]
    public class PolizaEstadoEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del estado de la póliza")]
        public Guid Id { get; private set; }

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(128)]
        [Comment("Descripción del estado de la póliza")]
        public string? Descripcion { get; set; }

        [Column(Order = 3, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }

        public virtual PolizaEntity? PolizaEntity { get; set; }
    }
}

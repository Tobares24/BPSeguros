using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("TipoPersonaTable", Schema = "PersonaSchema")]
    [Index(nameof(TipoPersona), IsUnique = false, Name = "TipoPersonaBusquedaIndex")]
    public class TipoPersonaEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador del catálogo de tipo de persona")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "El tipo de persona debe tener un máximo de 64 caracteres y un mínimo de 1 caracter")]
        [Comment("Tipo de persona")]
        public string? TipoPersona { get; set; }

        [Column(Order = 3, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }

        public virtual List<PersonaEntity>? Personas { get; set; }
    }
}
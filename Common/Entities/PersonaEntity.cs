using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("PersonaTable", Schema = "Persona")]
    [Index(nameof(CedulaAsegurado), IsUnique = true, Name = "PersonaIndex")]
    [Index(nameof(IdTipoPersona), IsUnique = false, Name = "IdTipoPersonaIndex")]
    [Index(nameof(Nombre), IsUnique = false, Name = "NombreIndex")]
    [Index(nameof(PrimerApellido), IsUnique = false, Name = "PrimerApellidoIndex")]
    [Index(nameof(SegundoApellido), IsUnique = false, Name = "SegundoApellidoIndex")]
    [Index(nameof(EstaEliminado), IsUnique = false, Name = "EstaEliminadoIndex")]
    public class PersonaEntity
    {
        [Key]
        [Column(Order = 1, TypeName = "VARCHAR")]
        [StringLength(64, MinimumLength = 1, ErrorMessage = "La cédula del asegurado debe tener un máximo de 64 caracteres y un mínimo de 1 caracter")]
        [Comment("Cédula del asegurado de la persona")]
        public string? CedulaAsegurado { get; set; }

        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(512, MinimumLength = 1, ErrorMessage = "El nombre debe tener un máximo de 512 caracteres y un mínimo de 1 caracter")]
        [Comment("Nombre de la persona")]
        public string? Nombre { get; set; }

        [Column(Order = 3, TypeName = "VARCHAR")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "El primer apellido debe tener un máximo de 128 caracteres y un mínimo de 1 caracter")]
        [Comment("Primer apellido de la persona")]
        public string? PrimerApellido { get; set; }

        [Column(Order = 4, TypeName = "VARCHAR")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "El segundo apellido debe tener un máximo de 128 caracteres y un mínimo de 1 caracter")]
        [Comment("Segundo apellido de la persona")]
        public string? SegundoApellido { get; set; }

        [Column(Order = 5, TypeName = "UNIQUEIDENTIFIER")]
        [Comment("Identificador de la persona con la que se relaciona la persona")]
        public Guid IdTipoPersona { get; set; }

        [ForeignKey(nameof(IdTipoPersona))]
        public TipoPersonaEntity? TipoPersona { get; set; }

        [Column(Order = 6, TypeName = "DATETIME")]
        [Comment("Fecha de nacimiento de la persona")]
        public DateTime? FechaNacimiento { get; set; }

        [Column(Order = 7, TypeName = "BIT")]
        [Comment("Indicador de borrado lógico")]
        public bool EstaEliminado { get; set; }

        [NotMapped]
        public virtual PolizaEntity? Poliza { get; set; }
    }
}
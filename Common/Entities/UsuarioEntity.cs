using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [Table("UsuarioTable", Schema = "SeguridadSchema")]
    public class UsuarioEntity
    {
        [Key]
        [Column(TypeName = "UNIQUEIDENTIFIER")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(512)]
        public string? PasswordHash { get; set; }
    }
}

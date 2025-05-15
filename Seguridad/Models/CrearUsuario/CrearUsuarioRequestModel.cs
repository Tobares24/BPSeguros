using System.ComponentModel.DataAnnotations;

namespace Seguridad.Models.CrearUsuario
{
    public class CrearUsuarioRequestModel
    {
        [MaxLength(256)]
        [Required]
        [EmailAddress(ErrorMessage = "El formato del email es incorrecto")]
        public string? Email { get; set; }

        [MaxLength(512)]
        [Required]
        public string? Password { get; set; }
    }
}

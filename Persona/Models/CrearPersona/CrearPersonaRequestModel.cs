using System.ComponentModel.DataAnnotations;

namespace Persona.Models.CrearPersona
{
    public class CrearPersonaRequestModel
    {
        [Required(ErrorMessage = "El número de cédula del asegurado es obligatorio.")]
        public string? CedulaAsegurado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{1,512}$", ErrorMessage = "El nombre solo puede contener letras y espacios, con una longitud de entre 1 y 512 caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{1,128}$", ErrorMessage = "El primer apellido solo puede contener letras y espacios, con una longitud de entre 1 y 128 caracteres.")]
        public string? PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{1,128}$", ErrorMessage = "El segundo apellido solo puede contener letras y espacios, con una longitud de entre 1 y 128 caracteres.")]
        public string? SegundoApellido { get; set; }

        [Required(ErrorMessage = "El identificador de tipo persona es obligatorio.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$", ErrorMessage = "El identificador de tipo persona debe tener un formato GUID válido.")]
        public Guid IdTipoPersona { get; set; }

        public DateTime? FechaNacimiento { get; set; }
    }
}
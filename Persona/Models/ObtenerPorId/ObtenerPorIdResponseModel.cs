namespace Persona.Models.ObtenerPorId
{
    public class ObtenerPorIdResponseModel
    {
        public string? CedulaAsegurado { get; set; }
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public Guid IdTipoPersona { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }
}

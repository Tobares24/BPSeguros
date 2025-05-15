using Common.Models;

namespace Persona.Models.ObtenerPersona
{
    public class ObtenerPersonaRequestModel : ObtenerRequestModel
    {
        public string? Nombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CedulaAsegurado { get; set; }
        public Guid? IdTipoPersona { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using Persona.Services.ActualizarPersona;
using Persona.Services.CrearPersona;
using Persona.Services.EliminarPersona;
using Persona.Services.ListaSelectorPersona;
using Persona.Services.ObtenerPersona;

namespace Persona.Controllers
{
    [Route("personas")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear([FromServices] CrearPersonaService service) => await service.Crear(HttpContext);

        [HttpGet()]
        public async Task<IActionResult> Obtener([FromServices] ObtenerPersonaService service) => await service.Obtener(HttpContext);

        [HttpGet("select")]
        public async Task<IActionResult> ListaSelectorPersona([FromServices] ListaSelectorPersonaService service) => await service.ListaSelectorPersona(HttpContext);

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar([FromServices] ActualizarPersonaService service) => await service.Actualizar(HttpContext);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar([FromServices] EliminarPersonaService service) => await service.Eliminar(HttpContext);
    }
}
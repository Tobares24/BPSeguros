using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poliza.Services;
using Poliza.Services.ActualizarPoliza;
using Poliza.Services.EliminarPoliza;
using Poliza.Services.ListaSelectorPolizaCobertura;
using Poliza.Services.ListaSelectorPolizaEstado;
using Poliza.Services.ListaSelectorTipoPoliza;
using Poliza.Services.ObtenerPoliza;
using Poliza.Services.ObtenerPorId;

namespace Poliza.Controllers
{
    [Route("api/polizas")]
    [ApiController]
    [Authorize]
    public class PolizaController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear([FromServices] CrearPolizaService service) => await service.Crear(HttpContext);

        [HttpGet]
        public async Task<IActionResult> Obtener([FromServices] ObtenerPolizaService service) => await service.Obtener(HttpContext);

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId([FromServices] ObtenerPorIdService service) => await service.ObtenerPorId(HttpContext);

        [HttpGet("select-estado")]
        public async Task<IActionResult> ListaSelectorPolizaEstado([FromServices] ListaSelectorPolizaEstadoService service) => await service.ListaSelectorPolizaEstado(HttpContext);

        [HttpGet("select-cobertura")]
        public async Task<IActionResult> ListaSelectorPolizaCobertura([FromServices] ListaSelectorPolizaCoberturaService service) => await service.ListaSelectorPolizaCobertura(HttpContext);

        [HttpGet("select-tipo-poliza")]
        public async Task<IActionResult> ListaSelectorTipoPoliza([FromServices] ListaSelectorTipoPolizaService service) => await service.ListaSelectorTipoPoliza(HttpContext);

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar([FromServices] ActualizarPolizaService service) => await service.Actualizar(HttpContext);

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar([FromServices] EliminarPolizaService service) => await service.Eliminar(HttpContext);
    }
}

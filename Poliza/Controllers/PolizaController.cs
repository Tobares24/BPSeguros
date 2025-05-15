using Microsoft.AspNetCore.Mvc;
using Poliza.Services;
using Poliza.Services.ListaSelectorPolizaCobertura;
using Poliza.Services.ListaSelectorPolizaEstado;
using Poliza.Services.ListaSelectorTipoPoliza;

namespace Poliza.Controllers
{
    [Route("api/polizas")]
    [ApiController]
    public class PolizaController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CrearPoliza([FromServices] CrearPolizaService service) => await service.CrearPoliza(HttpContext);

        [HttpGet("select-estado")]
        public async Task<IActionResult> ListaSelectorPolizaEstado([FromServices] ListaSelectorPolizaEstadoService service) => await service.ListaSelectorPolizaEstado(HttpContext);

        [HttpGet("select-cobertura")]
        public async Task<IActionResult> ListaSelectorPolizaCobertura([FromServices] ListaSelectorPolizaCoberturaService service) => await service.ListaSelectorPolizaCobertura(HttpContext);

        [HttpGet("select-tipo-poliza")]
        public async Task<IActionResult> ListaSelectorTipoPoliza([FromServices] ListaSelectorTipoPolizaService service) => await service.ListaSelectorTipoPoliza(HttpContext);
    }
}

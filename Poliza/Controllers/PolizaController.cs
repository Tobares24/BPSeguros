using Microsoft.AspNetCore.Mvc;
using Poliza.Services.ListaSelectorPolizaCobertura;
using Poliza.Services.ListaSelectorPolizaEstado;
using Poliza.Services.ListaSelectorPolizaPeriodo;
using Poliza.Services.ListaSelectorTipoPoliza;

namespace Poliza.Controllers
{
    [Route("api/polizas")]
    [ApiController]
    public class PolizaController : ControllerBase
    {
        [HttpGet("select-estado/{filtro}")]
        public async Task<IActionResult> ListaSelectorPolizaEstado([FromServices] ListaSelectorPolizaEstadoService service) => await service.ListaSelectorPolizaEstado(HttpContext);

        [HttpGet("select-cobertura/{filtro}")]
        public async Task<IActionResult> ListaSelectorPolizaCobertura([FromServices] ListaSelectorPolizaCoberturaService service) => await service.ListaSelectorPolizaCobertura(HttpContext);

        [HttpGet("select-tipo-poliza/{filtro}")]
        public async Task<IActionResult> ListaSelectorTipoPoliza([FromServices] ListaSelectorTipoPolizaService service) => await service.ListaSelectorTipoPoliza(HttpContext);

        [HttpGet("select-periodo/{filtro}")]
        public async Task<IActionResult> ListaSelectorPolizaPeriodo([FromServices] ListaSelectorPolizaPeriodoService service) => await service.ListaSelectorPolizaPeriodo(HttpContext);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguridad.Services.CrearUsuario;
using Seguridad.Services.Login;

namespace Seguridad.Controllers
{
    [Route("api/seguridad")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Crear([FromServices] CrearUsuarioService service) => await service.Crear(HttpContext);

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromServices] LoginService service) => await service.Login(HttpContext);
    }
}

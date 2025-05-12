using Coworking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("ObterUsuarios")]
        public async Task<IActionResult> ObterUsuarios()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("ObterUsuarioPorId/{id}")]
        public async Task<IActionResult> ObterUsuarioPorId(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario is null) return NotFound();
            return Ok(usuario);
        }
    }

}

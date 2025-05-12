using Coworking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase
    {
        private readonly ISalaService _salaService;

        public SalaController(ISalaService salaService)
        {
            _salaService = salaService;
        }

        [HttpGet("ObterSalas")]
        public async Task<IActionResult> ObterSalas()
        {
            var salas = await _salaService.ObterTodosAsync();
            return Ok(salas);
        }

        [HttpGet("ObterSalaPorId/{id}")]
        public async Task<IActionResult> ObterSalaPorId(Guid id)
        {
            var sala = await _salaService.ObterPorIdAsync(id);
            if (sala is null) return NotFound();
            return Ok(sala);
        }
    }

}

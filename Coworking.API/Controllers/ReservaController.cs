using Coworking.API.DTOs;
using Coworking.Application.Interfaces;
using Coworking.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Coworking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IEmailService _emailService;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReservaController(IReservaService reservaService, IEmailService emailService, IHttpClientFactory httpClientFactory)
        {
            _reservaService = reservaService;
            _emailService = emailService;
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet("ObterTodas")]
        public async Task<IActionResult> ObterTodasReservas()
        {
            try
            {
                var reservas = await _reservaService.ObterTodasReservasAsync();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter reservas: {ex.Message}");
            }
        }

        [HttpGet("ObterPorId/{id}")]
        public async Task<IActionResult> ObterReservasPorId(Guid id)
        {
            try
            {
                var reserva = await _reservaService.ObterPorIdAsync(id);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter reserva por ID: {ex.Message}");
            }
        }

        [HttpPost("Criar")]
        public async Task<IActionResult> CriarReserva(CriarReservaDto reservaDto)
        {
            try
            {
                var reserva = new Reserva
                {
                    SalaId = reservaDto.SalaId,
                    UsuarioId = reservaDto.UsuarioId,
                    DataHoraReserva = reservaDto.DataHoraReserva,
                    Status = StatusReserva.Confirmada
                };

                var sucesso = await _reservaService.CriarReservaAsync(reserva);

                if (!sucesso)
                    return BadRequest("Horário já reservado.");

                var mensagem = MontaEmail("criada",
                                          reservaDto.NomeUsuario,
                                          reservaDto.NomeSala,
                                          reservaDto.DataHoraReserva);

                await EnviarConfirmacaoEmailAsync(reservaDto.EmailUsuario, "Confirmação de Reserva", mensagem);

                return CreatedAtAction(nameof(ObterTodasReservas), new { id = reserva.Id }, reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar reserva: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> EditarReserva(Guid id, [FromBody] EditarReservaDto reservaDto)
        {
            try
            {
 
                var reservaExistente = await _reservaService.ObterPorIdAsync(id);
                if (reservaExistente == null)
                    return NotFound("Reserva não encontrada.");

                reservaExistente.DataHoraReserva = reservaDto.DataHoraReserva;
                reservaExistente.SalaId = reservaDto.SalaId;

                var sucesso = await _reservaService.EditarReservaAsync(reservaExistente);
                if (!sucesso)
                    return BadRequest("Falha ao editar a reserva.");

                var mensagem = MontaEmail("editada",
                                          reservaDto.NomeUsuario,
                                          reservaDto.NomeSala,
                                          reservaDto.DataHoraReserva);

                await EnviarConfirmacaoEmailAsync(reservaDto.EmailUsuario, "Edição de Reserva", mensagem);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao editar reserva: {ex.Message}");
            }
        }

        [HttpDelete("Cancelar/{id}")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            try
            {
                var reserva = await _reservaService.ObterPorIdAsync(id);

                var sucesso = await _reservaService.CancelarReservaAsync(id);
                if (!sucesso)
                    return BadRequest("A reserva só pode ser cancelada com 24h de antecedência.");

                var mensagem = MontaEmail("cancelada",
                                          reserva.Usuario.Nome,
                                          reserva.Sala.Nome,
                                          reserva.DataHoraReserva);

                await EnviarConfirmacaoEmailAsync(reserva.Usuario.Email, "Cancelamento de Reserva", mensagem);

                return Ok("Reserva cancelada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cancelar reserva: {ex.Message}");
            }
        }


        private static string MontaEmail(string acao, string nome, string sala, DateTime data)
        {
            var mensagem = $@"
            <h2>Reserva {acao} com sucesso!</h2>
            <p><strong>Usuário:</strong> {nome}</p>
            <p><strong>Sala:</strong> {sala}</p>
            <p><strong>Data e Hora:</strong> {data:dd/MM/yyyy HH:mm}</p>";

            return mensagem;
        }

        private async Task EnviarConfirmacaoEmailAsync(string email, string assunto, string corpo)
        {
            var client = _httpClientFactory.CreateClient();

            var url = $"https://localhost:7065/email/enviaemail" +
                      $"?email={Uri.EscapeDataString(email)}" +
                      $"&subject={Uri.EscapeDataString(assunto)}" +
                      $"&body={Uri.EscapeDataString(corpo)}";

            await client.GetAsync(url);
        }
    }
}

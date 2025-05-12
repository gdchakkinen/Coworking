using Coworking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coworking.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IEmailService _emailService;

        public EmailController(ILogger<EmailController> logger,
                                IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("EnviaEmail")]
        public async Task<IActionResult> TestEmail(string email, string
                                                   subject, string body)
        {
            try
            {
                await _emailService.EnviaEmailAsync(email, subject, body);
                _logger.LogInformation($"{StatusCodes.Status200OK} - Email enviado com sucesso");
              return Ok("Email enviado com sucesso !!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Erro ao enviar o email");
            }
        }
    }
}

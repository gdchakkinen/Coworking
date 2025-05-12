using Coworking.Application.Validations;
using System.ComponentModel.DataAnnotations;

namespace Coworking.API.DTOs
{
    public class CriarReservaDto
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "O nome da sala é obrigatório.")]
        public string NomeSala { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        public string EmailUsuario { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "A data/hora da reserva é obrigatória.")]
        [FutureHourOnly]
        public DateTime DataHoraReserva { get; set; }

        [Required(ErrorMessage = "O ID da sala é obrigatório.")]
        public Guid SalaId { get; set; }
    }

}

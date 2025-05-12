
using System.Text.Json.Serialization;

namespace Coworking.Domain.Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}

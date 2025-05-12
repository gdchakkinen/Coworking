
using System.Text.Json.Serialization;

namespace Coworking.Domain.Entidades
{
    public class Sala
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nome { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public int Capacidade { get; set; }

        [JsonIgnore]
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    }
}


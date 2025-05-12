namespace Coworking.Domain.Entidades
{
    public enum StatusReserva
    {
        Confirmada,
        Cancelada
    }

    public class Reserva
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SalaId { get; set; }

        public Guid UsuarioId { get; set; }

        public DateTime DataHoraReserva { get; set; }

        public StatusReserva Status { get; set; } = StatusReserva.Confirmada;

        public Sala Sala { get; set; }

        public Usuario Usuario { get; set; }
    }
}

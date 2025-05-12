namespace Coworking.MVC.Models
{
    public class ReservaViewModel
    {
        public Guid Id { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public SalaViewModel Sala { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public Guid SalaId { get; set; }
        public Guid UsuarioId { get; set; }
    }
}

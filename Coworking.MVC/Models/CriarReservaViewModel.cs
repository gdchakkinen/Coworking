namespace Coworking.MVC.Models
{
    public class CriarReservaViewModel
    {
        public string NomeUsuario { get; set; }
        public string NomeSala { get; set; }
        public string EmailUsuario { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public Guid SalaId { get; set; }
    }
}

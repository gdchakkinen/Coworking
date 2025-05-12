using Coworking.Domain.Entidades;
using System.Threading.Tasks;

namespace Coworking.Application.Interfaces
{
    public interface IReservaService
    {
        Task<Reserva> ObterPorIdAsync(Guid id);
        Task<List<Reserva>> ObterTodasReservasAsync();
        Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataHora);
        Task<bool> EditarReservaAsync(Reserva reserva);
        Task<bool> PodeCancelarAsync(Guid id);
        Task<bool> CancelarReservaAsync(Guid id);
        Task<bool> CriarReservaAsync(Reserva reserva);
    }
}

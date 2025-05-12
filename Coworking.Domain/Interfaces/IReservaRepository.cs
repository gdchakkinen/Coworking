
using Coworking.Domain.Entidades;

namespace Coworking.Domain.Interfaces
{
    public interface IReservaRepository
    {
        Task<Reserva?> ObterPorIdAsync(Guid id);
        Task<List<Reserva>> ListarTodasReservasAsync();
        Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataHora);
        Task AdicionarAsync(Reserva reserva);
        Task<Reserva> PodeCancelarAsync(Guid id);
        void Remover(Guid id);
        void Atualizar(Reserva reserva);
    }
}


namespace Coworking.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IReservaRepository Reservas { get; }
        ISalaRepository Salas { get; }
        IUsuarioRepository Usuarios { get; }

        Task<bool> CommitAsync();
    }
}

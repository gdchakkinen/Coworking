using Coworking.Domain.Entidades;

namespace Coworking.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task<Usuario?> ObterPorIdAsync(Guid id);
    }

}

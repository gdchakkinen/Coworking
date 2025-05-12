using Coworking.Domain.Entidades;

namespace Coworking.Application.Interfaces
{
    public interface ISalaService
    {
        Task<IEnumerable<Sala>> ObterTodosAsync();
        Task<Sala?> ObterPorIdAsync(Guid id);
    }

}

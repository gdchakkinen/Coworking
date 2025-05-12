
using Coworking.Domain.Entidades;

namespace Coworking.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(Guid id);
        Task AddAsync(Usuario usuario);
        void Update(Usuario usuario);
        void Remove(Guid id);
    }
}

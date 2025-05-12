
using Coworking.Domain.Entidades;

namespace Coworking.Domain.Interfaces
{
    public interface ISalaRepository
    {
        Task<IEnumerable<Sala>> GetAllAsync();
        Task<Sala?> GetByIdAsync(Guid id);
        Task AddAsync(Sala sala);
        void Update(Sala sala);
        void Remove(Guid id);
    }
}

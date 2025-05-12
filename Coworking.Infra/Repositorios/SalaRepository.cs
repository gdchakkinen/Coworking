
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;
using Coworking.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Infra.Repositorios
{
    public class SalaRepository : ISalaRepository
    {
        private readonly AppDbContext _context;

        public SalaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sala>> GetAllAsync()
            => await _context.Salas.ToListAsync();

        public async Task<Sala?> GetByIdAsync(Guid id)
            => await _context.Salas.FindAsync(id);

        public async Task AddAsync(Sala sala)
            => await _context.Salas.AddAsync(sala);

        public void Update(Sala sala)
            => _context.Salas.Update(sala);

        public void Remove(Guid id)
        {
            var sala = new Sala { Id = id };
            _context.Salas.Remove(sala);
        }
    }
}

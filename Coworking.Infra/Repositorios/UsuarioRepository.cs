
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;
using Coworking.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Infra.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
            
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
            => await _context.Usuarios.ToListAsync();

        public async Task<Usuario?> GetByIdAsync(Guid id)
            => await _context.Usuarios.FindAsync(id);

        public async Task AddAsync(Usuario usuario)
            => await _context.Usuarios.AddAsync(usuario);

        public void Update(Usuario usuario)
            => _context.Usuarios.Update(usuario);

        public void Remove(Guid id)
        {
            var usuario = new Usuario { Id = id };
            _context.Usuarios.Remove(usuario);
        }
    }
}

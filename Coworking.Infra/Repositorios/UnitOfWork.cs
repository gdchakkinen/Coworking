using Coworking.Domain.Interfaces;
using Coworking.Infra.Data;

namespace Coworking.Infra.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IReservaRepository Reservas { get; }
        public ISalaRepository Salas { get; }
        public IUsuarioRepository Usuarios { get; }

        public UnitOfWork(AppDbContext context, IReservaRepository reservaRepository, ISalaRepository salaRepository, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            Reservas = reservaRepository;
            Salas = salaRepository;
            Usuarios = usuarioRepository;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

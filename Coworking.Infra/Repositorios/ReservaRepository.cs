
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;
using Coworking.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Coworking.Infra.Repositorios
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly AppDbContext _context;

        public ReservaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva?> ObterPorIdAsync(Guid id)
        {
            try
            {
                return await _context.Reservas
                            .Where(r => r.Status == StatusReserva.Confirmada)
                            .Include(r => r.Sala)
                            .Include(r => r.Usuario)
                            .FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Reserva>> ListarTodasReservasAsync()
        {
            return await _context.Reservas
                .Where(r => r.Status == StatusReserva.Confirmada)
                .Include(r => r.Usuario)
                .Include(r => r.Sala)
                .ToListAsync();
        }

        public async Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataHora)
        {
            try
            {
                var retorno = await _context.Reservas.AnyAsync(r => r.SalaId == salaId && r.DataHoraReserva == dataHora);
                return retorno;
            }
            catch (Exception e)
            {
                var mensagem = e.Message;
                throw;
            }
        }

        public async Task AdicionarAsync(Reserva reserva)
        {
            try
            {
                await _context.Reservas.AddAsync(reserva);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Atualizar(Reserva reserva)
        {
            try
            {
                _context.Reservas.Update(reserva);        
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Reserva> PodeCancelarAsync(Guid id)
        {
            try
            {
                var entity = await _context.Reservas.FindAsync(id) ??
                                    throw new Exception("Id nao encontrado");
            
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public void Remover(Guid id)
        {
            try
            {
                var reserva = _context.Reservas.FirstOrDefault(r => r.Id == id);
                if (reserva != null)
                {
                    reserva.Status = StatusReserva.Cancelada;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


using Coworking.Application.Interfaces;
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;

namespace Coworking.Application.Servicos
{
    public class ReservaService : IReservaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Reserva> ObterPorIdAsync(Guid id)
        {
            try
            {
                var retorno = await _unitOfWork.Reservas.ObterPorIdAsync(id);
                if (retorno != null)
                    return retorno;

                throw new Exception("Reserva não encontrada.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter reserva por ID: {ex.Message}", ex);
            }
        }

        public async Task<List<Reserva>> ObterTodasReservasAsync()
        {
            try
            {
                return await _unitOfWork.Reservas.ListarTodasReservasAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar reservas: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExisteConflitoReservaAsync(Guid salaId, DateTime dataHora)
        {
            try
            {
                return await _unitOfWork.Reservas.ExisteConflitoReservaAsync(salaId, dataHora);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar conflito de reserva: {ex.Message}", ex);
            }
        }

        public async Task<bool> CriarReservaAsync(Reserva reserva)
        {
            try
            {
                var conflito = await ExisteConflitoReservaAsync(reserva.SalaId, reserva.DataHoraReserva);
                if (conflito)
                    return false;

                await _unitOfWork.Reservas.AdicionarAsync(reserva);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar reserva: {ex.Message}", ex);
            }
        }

        public async Task<bool> EditarReservaAsync(Reserva reserva)
        {
            try
            {
                var dadosReservaAnterior = await _unitOfWork.Reservas.ObterPorIdAsync(reserva.Id);
                if (dadosReservaAnterior == null)
                    return false;

                dadosReservaAnterior.DataHoraReserva = reserva.DataHoraReserva;
                dadosReservaAnterior.UsuarioId = reserva.UsuarioId;
                dadosReservaAnterior.Id = reserva.Id;

                _unitOfWork.Reservas.Atualizar(dadosReservaAnterior);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao editar reserva: {ex.Message}", ex);
            }
        }

        public async Task<bool> PodeCancelarAsync(Guid id)
        {
            try
            {
                var reserva = await ObterPorIdAsync(id);
                var dataHoraAtual = DateTime.Now;

                return dataHoraAtual.AddHours(24) <= reserva.DataHoraReserva;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar possibilidade de cancelamento: {ex.Message}", ex);
            }
        }

        public async Task<bool> CancelarReservaAsync(Guid id)
        {
            try
            {
                if (!await PodeCancelarAsync(id))
                    return false;

                _unitOfWork.Reservas.Remover(id);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cancelar reserva: {ex.Message}", ex);
            }
        }

    }
}

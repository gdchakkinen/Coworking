using Coworking.Application.Interfaces;
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;

namespace Coworking.Application.Servicos
{
    public class SalaService : ISalaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Sala>> ObterTodosAsync()
            => await _unitOfWork.Salas.GetAllAsync();

        public async Task<Sala?> ObterPorIdAsync(Guid id)
            => await _unitOfWork.Salas.GetByIdAsync(id);
    }
}

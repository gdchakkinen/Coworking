using Coworking.Application.Interfaces;
using Coworking.Domain.Entidades;
using Coworking.Domain.Interfaces;

namespace Coworking.Application.Servicos
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
            => await _unitOfWork.Usuarios.GetAllAsync();

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
            => await _unitOfWork.Usuarios.GetByIdAsync(id);
    }

}

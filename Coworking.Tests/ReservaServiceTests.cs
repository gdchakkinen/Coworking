using Moq;
using Coworking.Domain.Entidades;
using Coworking.Application.Servicos;
using Coworking.Domain.Interfaces;
using Coworking.Application.Interfaces;
using Xunit.Abstractions;

namespace Coworking.Tests
{
    public class ReservaServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReservaRepository> _mockReservaRepository;
        private readonly IReservaService _reservaService;
        private readonly ITestOutputHelper _output;

        public ReservaServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockReservaRepository = new Mock<IReservaRepository>();
            _mockUnitOfWork.Setup(u => u.Reservas).Returns(_mockReservaRepository.Object);
            _reservaService = new ReservaService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CriarReservaAsync_DeveRetornarTrue_QuandoNaoHouverConflito()
        {
            // Arrange
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                SalaId = Guid.NewGuid(),
                DataHoraReserva = DateTime.Now.AddHours(2)
            };

            _mockReservaRepository.Setup(r => r.ExisteConflitoReservaAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                                  .ReturnsAsync(false);

            _mockReservaRepository.Setup(r => r.AdicionarAsync(It.IsAny<Reserva>())).Verifiable();

            _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(true).Verifiable();

            // Act
            var resultado = await _reservaService.CriarReservaAsync(reserva);

            // Assert
            Assert.True(resultado);
            _mockReservaRepository.Verify(r => r.AdicionarAsync(It.IsAny<Reserva>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CriarReservaAsync_DeveRetornarFalse_QuandoHouverConflito()
        {
            // Arrange
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                SalaId = Guid.NewGuid(),
                DataHoraReserva = DateTime.Now.AddHours(2)
            };

            _mockReservaRepository.Setup(r => r.ExisteConflitoReservaAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                                  .ReturnsAsync(true);

            // Act
            var resultado = await _reservaService.CriarReservaAsync(reserva);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task PodeCancelarAsync_DeveRetornarTrue_QuandoTiverMaisDe24Horas()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DataHoraReserva = now.AddHours(25),
                Status = StatusReserva.Confirmada
            };

            _mockReservaRepository
                .Setup(r => r.ObterPorIdAsync(reserva.Id))
                .ReturnsAsync(reserva);

            _output.WriteLine($"NOW (UTC): {now}");
            _output.WriteLine($"RESERVA: {reserva.DataHoraReserva}");
            _output.WriteLine($"DIFERENÇA EM HORAS: {(reserva.DataHoraReserva - now).TotalHours}");

            // Act
            var resultado = await _reservaService.PodeCancelarAsync(reserva.Id);

            _output.WriteLine($"RESULTADO: {resultado}");

            // Assert
            Assert.True(resultado);
        }


        [Fact]
        public async Task PodeCancelarAsync_DeveRetornarFalse_QuandoTiverMenosDe24Horas()
        {
            // Arrange
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DataHoraReserva = DateTime.Now.AddHours(12) // Menos de 24 horas
            };

            _mockReservaRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(reserva);

            // Act
            var resultado = await _reservaService.PodeCancelarAsync(reserva.Id);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task CancelarReservaAsync_DeveRetornarFalse_QuandoNaoPuderCancelar()
        {
            // Arrange
            var reserva = new Reserva
            {
                Id = Guid.NewGuid(),
                DataHoraReserva = DateTime.Now.AddHours(12) // Menos de 24 horas
            };

            _mockReservaRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(reserva);

            // Act
            var resultado = await _reservaService.CancelarReservaAsync(reserva.Id);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task CancelarReservaAsync_DeveRetornarTrue_QuandoPuderCancelar()
        {
            // Arrange
            var reservaId = Guid.NewGuid();
            var dataHoraReserva = DateTime.UtcNow.AddHours(25);

            var reserva = new Reserva
            {
                Id = reservaId,
                DataHoraReserva = dataHoraReserva,
                Status = StatusReserva.Confirmada
            };

            _mockReservaRepository
                .Setup(r => r.ObterPorIdAsync(reservaId))
                .ReturnsAsync(reserva);

            _mockReservaRepository
                .Setup(r => r.Remover(reservaId))
                .Verifiable();

            _mockUnitOfWork
                .Setup(u => u.CommitAsync())
                .ReturnsAsync(true)
                .Verifiable();

            // Act
            var resultado = await _reservaService.CancelarReservaAsync(reservaId);

            // Assert
            Assert.True(resultado);
            _mockReservaRepository.Verify(r => r.Remover(reservaId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        }

    }
}

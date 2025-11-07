using Challenger.Application.DTOs.Requests;
using Challenger.Domain.Entities;
using Challenger.Domain.Interfaces;
using Moq;

namespace TestProject1.Application.UseCases;

public class UpdatePatioUseCaseTests
{
    private readonly Mock<IPatioRepository> _mockRepository;
        private readonly UpdatePatioUseCase _useCase;

        public UpdatePatioUseCaseTests()
        {
            _mockRepository = new Mock<IPatioRepository>();
            _useCase = new UpdatePatioUseCase(_mockRepository.Object);
        }

        [Fact(DisplayName = "Deve atualizar o pátio existente e retornar true")]
        public async Task Execute_DeveAtualizarPatioExistente()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PatioRequest
            {
                Name = "Pátio Central",
                Cidade = "São Paulo",
                Capacidade = 200
            };

            var patioExistente = new Patio("Antigo Pátio", "Campinas", 100, "user_antigo");

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(patioExistente);

            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Patio>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _useCase.Execute(id, request, "admin");

            // Assert
            Assert.True(resultado);
            Assert.Equal("Pátio Central", patioExistente.Name);
            Assert.Equal("São Paulo", patioExistente.Cidade);
            Assert.Equal(200, patioExistente.Capacidade);
            Assert.Equal("admin", patioExistente.UpdatedBy);

            _mockRepository.Verify(r => r.UpdateAsync(patioExistente), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar false se o pátio não existir")]
        public async Task Execute_DeveRetornarFalse_SePatioNaoExistir()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PatioRequest { Name = "Novo Pátio", Cidade = "Rio", Capacidade = 150 };

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Patio)null);

            // Act
            var resultado = await _useCase.Execute(id, request, "admin");

            // Assert
            Assert.False(resultado);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Patio>()), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar exceção se ocorrer erro no repositório")]
        public async Task Execute_DeveLancarExcecao_SeRepositorioFalhar()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PatioRequest { Name = "Falha", Cidade = "Erro", Capacidade = 100 };
            var patio = new Patio("Antigo", "Campinas", 50, "user");

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(patio);

            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Patio>()))
                .ThrowsAsync(new Exception("Erro no banco"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(id, request, "admin"));
        }
}
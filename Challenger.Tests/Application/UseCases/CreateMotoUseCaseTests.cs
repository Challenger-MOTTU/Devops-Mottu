using Challenger.Application.DTOs.Requests;
using Challenger.Application.UseCase.CreateMoto;
using Challenger.Domain;
using Challenger.Domain.Entities;
using Challenger.Domain.Enum;
using Challenger.Domain.Interfaces;
using Moq;

namespace TestProject1.Application.UseCases;

public class CreateMotoUseCaseTests
{
    [Fact]
    public async Task Execute_DeveCriarMotoComSucesso()
    {
        // Arrange
        var mockRepo = new Mock<IMotoRepository>();
        var useCase = new CreateMotoUseCase(mockRepo.Object);

        var request = new MotoRequest
        {
            Placa = "ABC-1234",
            Modelo = ModeloMoto.SPORT, // <-- enum
            Status = StatusMoto.DISPONIVEL, // <-- enum
            PatioId = Guid.NewGuid()
        };

        var createdBy = "admin";

        // Configura o mock do repositório
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Moto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await useCase.Execute(request, createdBy);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Placa, result.Placa);
        Assert.Equal(request.Modelo, result.Modelo);
        Assert.Equal(request.Status, result.Status);
        Assert.Equal(request.PatioId, result.PatioId);

        // Verifica se o AddAsync foi chamado exatamente uma vez
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Moto>()), Times.Once);
    }
}
using FluentAssertions;
using MediatR;
using Moq;
using TransferenciaAPI.Application.Commands;
using TransferenciaAPI.Application.Handlers;
using TransferenciaAPI.Domain.Entities;
using TransferenciaAPI.Domain.Interfaces;
using Xunit;

namespace TransferenciaAPI.Tests
{
    public class TransferenciaCommandHandlerTests
    {
        [Fact]
        public async Task Transferencia_Valida_DeveRealizarDebitoCreditoEPersistir()
        {
            var mockService = new Mock<IContaCorrenteService>();
            var mockRepo = new Mock<ITransferenciaRepository>();

            var origemId = "guid-origem";
            var destinoId = "guid-destino";

            mockService.Setup(s => s.ObterIdPorNumeroAsync("123")).ReturnsAsync(origemId);
            mockService.Setup(s => s.ObterIdPorNumeroAsync("456")).ReturnsAsync(destinoId);

            mockRepo.Setup(r => r.ExisteComIdempotentKeyAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(false);

            var handler = new TransferenciaCommandHandler(mockService.Object, mockRepo.Object);

            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123",
                ContaDestinoNumero = "456",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            mockService.Verify(s => s.DebitarAsync("123", 100.0m), Times.Once);
            mockService.Verify(s => s.CreditarAsync("456", 100.0m), Times.Once);
            mockRepo.Verify(r => r.RegistrarAsync(It.Is<Transferencia>(
                t => t.ContaOrigemId == origemId &&
                     t.ContaDestinoId == destinoId &&
                     t.Valor == 100.0m &&
                     t.IdempotentKey == "REQ-001")), Times.Once);

            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Deve_Falhar_QuandoValorMenorOuIgualZero()
        {
            var mockService = new Mock<IContaCorrenteService>();
            var mockRepo = new Mock<ITransferenciaRepository>();

            var handler = new TransferenciaCommandHandler(mockService.Object, mockRepo.Object);

            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123",
                ContaDestinoNumero = "456",
                Valor = 0,
                IdempotentKey = "REQ-001"
            };

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Valor da transferência deve ser maior que zero.");
        }

        [Fact]
        public async Task Deve_Falhar_QuandoContasOrigemDestinoIguais()
        {
            var mockService = new Mock<IContaCorrenteService>();
            var mockRepo = new Mock<ITransferenciaRepository>();

            var handler = new TransferenciaCommandHandler(mockService.Object, mockRepo.Object);

            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123",
                ContaDestinoNumero = "123",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Conta de origem e destino não podem ser iguais.");
        }

        [Fact]
        public async Task Deve_Falhar_QuandoIdempotentKeyDuplicada()
        {
            var mockService = new Mock<IContaCorrenteService>();
            var mockRepo = new Mock<ITransferenciaRepository>();

            var origemId = "guid-origem";
            var destinoId = "guid-destino";

            mockService.Setup(s => s.ObterIdPorNumeroAsync("123")).ReturnsAsync(origemId);
            mockService.Setup(s => s.ObterIdPorNumeroAsync("456")).ReturnsAsync(destinoId);

            mockRepo.Setup(r => r.ExisteComIdempotentKeyAsync(origemId, "REQ-001"))
                    .ReturnsAsync(true);

            var handler = new TransferenciaCommandHandler(mockService.Object, mockRepo.Object);

            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123",
                ContaDestinoNumero = "456",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Transferência duplicada detectada (IdempotentKey já utilizado).");

            mockService.Verify(s => s.DebitarAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
            mockService.Verify(s => s.CreditarAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
            mockRepo.Verify(r => r.RegistrarAsync(It.IsAny<Transferencia>()), Times.Never);
        }
    }
}

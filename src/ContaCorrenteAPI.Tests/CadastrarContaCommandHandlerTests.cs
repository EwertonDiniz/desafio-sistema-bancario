using FluentAssertions;
using MediatR;
using Moq;
using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Application.Handlers;
using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using Xunit;

namespace ContaCorrenteAPI.Tests.Application.Handlers
{
    public class CadastrarContaCommandHandlerTests
    {
        private readonly Mock<IContaCorrenteRepository> _mockRepo;
        private readonly CadastrarContaCommandHandler _handler;

        public CadastrarContaCommandHandlerTests()
        {
            _mockRepo = new Mock<IContaCorrenteRepository>();
            _handler = new CadastrarContaCommandHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Deve_CriarContaComSucesso_QuandoDadosValidos()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            _mockRepo.Setup(r => r.ObterPorCpfAsync(command.Cpf)).ReturnsAsync((ContaCorrente)null);
            _mockRepo.Setup(r => r.CriarAsync(It.IsAny<ContaCorrente>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNullOrEmpty();
            result.Length.Should().Be(9); // NumeroConta has 9 digits

            _mockRepo.Verify(r => r.ObterPorCpfAsync(command.Cpf), Times.Once);
            _mockRepo.Verify(r => r.CriarAsync(It.Is<ContaCorrente>(c =>
                c.Cpf == command.Cpf &&
                c.NomeTitular == command.NomeTitular &&
                c.Ativo == true &&
                !string.IsNullOrEmpty(c.SenhaHash) &&
                !string.IsNullOrEmpty(c.Salt))), Times.Once);
        }

        [Fact]
        public async Task Deve_LancarExcecao_QuandoCpfJaExiste()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var contaExistente = new ContaCorrente { Cpf = command.Cpf };
            _mockRepo.Setup(r => r.ObterPorCpfAsync(command.Cpf)).ReturnsAsync(contaExistente);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("DUPLICATE_CPF: Já existe uma conta com esse CPF.");

            _mockRepo.Verify(r => r.CriarAsync(It.IsAny<ContaCorrente>()), Times.Never);
        }
    }
}
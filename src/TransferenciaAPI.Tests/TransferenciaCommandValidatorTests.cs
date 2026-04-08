using FluentValidation.TestHelper;
using Xunit;
using TransferenciaAPI.Application.Commands;
using TransferenciaAPI.Application.Validators;

namespace TransferenciaAPI.Tests.Application.Validators
{
    public class TransferenciaCommandValidatorTests
    {
        private readonly TransferenciaCommandValidator _validator = new();

        [Fact]
        public void Deve_SerValido_QuandoDadosValidos()
        {
            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123456",
                ContaDestinoNumero = "654321",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Deve_Falhar_QuandoContaDestinoVazia()
        {
            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123456",
                ContaDestinoNumero = "",
                Valor = 100.0m,
                IdempotentKey = "REQ-001"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ContaDestinoNumero);
        }

        [Fact]
        public void Deve_Falhar_QuandoValorMenorOuIgualZero()
        {
            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123456",
                ContaDestinoNumero = "654321",
                Valor = 0,
                IdempotentKey = "REQ-001"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Deve_Falhar_QuandoValorNegativo()
        {
            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123456",
                ContaDestinoNumero = "654321",
                Valor = -10.0m,
                IdempotentKey = "REQ-001"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Deve_Falhar_QuandoIdempotentKeyVazia()
        {
            var command = new TransferenciaCommand
            {
                ContaOrigemNumero = "123456",
                ContaDestinoNumero = "654321",
                Valor = 100.0m,
                IdempotentKey = ""
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.IdempotentKey);
        }
    }
}
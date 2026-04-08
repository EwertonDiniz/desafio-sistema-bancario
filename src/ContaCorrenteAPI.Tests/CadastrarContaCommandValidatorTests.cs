using FluentValidation.TestHelper;
using Xunit;
using ContaCorrenteAPI.Application.Commands;

namespace ContaCorrenteAPI.Tests.Application.Validators
{
    public class CadastrarContaCommandValidatorTests
    {
        private readonly CadastrarContaCommandValidator _validator = new();

        [Fact]
        public void Deve_SerValido_QuandoDadosValidos()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Deve_Falhar_QuandoCpfVazio()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Deve_Falhar_QuandoCpfMenorQue11Digitos()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "123456789",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Deve_Falhar_QuandoCpfMaiorQue11Digitos()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "123456789012",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Deve_Falhar_QuandoCpfContemLetras()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "1234567890a",
                Senha = "senha123",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Deve_Falhar_QuandoSenhaVazia()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Senha);
        }

        [Fact]
        public void Deve_Falhar_QuandoSenhaMenorQue6Caracteres()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "12345",
                NomeTitular = "João Silva"
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Senha);
        }

        [Fact]
        public void Deve_Falhar_QuandoNomeTitularVazio()
        {
            var command = new CadastrarContaCommand
            {
                Cpf = "12345678901",
                Senha = "senha123",
                NomeTitular = ""
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NomeTitular);
        }
    }
}
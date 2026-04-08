using System.Net.Http.Json;
using ContaCorrenteAPI.Application.Commands;
using FluentAssertions;
using Xunit;

namespace ContaCorrenteAPI.IntegrationTests
{
    public class Response
    {
        public string NumeroConta { get; set; } = string.Empty;
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }

    public class AuthControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5001") };
        }

        [Fact]
        public async Task CadastrarConta_DeveFalhar_QuandoCpfDuplicado()
        {
            var command1 = new CadastrarContaCommand
            {
                Cpf = "98765432100",
                Senha = "senha123",
                NomeTitular = "Maria Souza"
            };
            var command2 = new CadastrarContaCommand
            {
                Cpf = "98765432100", // Mesmo CPF
                Senha = "outra123",
                NomeTitular = "José Santos"
            };

            await _client.PostAsJsonAsync("/api/auth/cadastrar", command1); // Criar primeira
            var response = await _client.PostAsJsonAsync("/api/auth/cadastrar", command2); // Tentar duplicada

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            result.Should().NotBeNull();
            result.Message.Should().Contain("DUPLICATE_CPF");
        }
    }
}
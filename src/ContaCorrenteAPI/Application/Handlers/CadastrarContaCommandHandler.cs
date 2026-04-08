
using ContaCorrenteAPI.Application.Commands;
using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Text.RegularExpressions;
using BCrypt.Net;

namespace ContaCorrenteAPI.Application.Handlers
{
    public class CadastrarContaCommandHandler : IRequestHandler<CadastrarContaCommand, string>
    {
        private readonly IContaCorrenteRepository _repository;

        public CadastrarContaCommandHandler(IContaCorrenteRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CadastrarContaCommand request, CancellationToken cancellationToken)
        {
            // Validar se CPF já existe
            var contaExistente = await _repository.ObterPorCpfAsync(request.Cpf);
            if (contaExistente != null)
                throw new ValidationException("DUPLICATE_CPF: Já existe uma conta com esse CPF.");

            var conta = new ContaCorrente
            {
                NomeTitular = request.NomeTitular,
                Cpf = request.Cpf,
                NumeroConta = new Random().Next(100000000, 999999999).ToString(),
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Salt = BCrypt.Net.BCrypt.GenerateSalt(),
                Ativo = true
            };

            await _repository.CriarAsync(conta);
            return conta.NumeroConta;
        }
    }
}

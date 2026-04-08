using MediatR;
using TransferenciaAPI.Application.Commands;
using TransferenciaAPI.Domain.Entities;
using TransferenciaAPI.Domain.Interfaces;

namespace TransferenciaAPI.Application.Handlers
{
    public class TransferenciaCommandHandler : IRequestHandler<TransferenciaCommand, Unit>
    {
        private readonly IContaCorrenteService _contaService;
        private readonly ITransferenciaRepository _repository;

        public TransferenciaCommandHandler(IContaCorrenteService contaService, ITransferenciaRepository repository)
        {
            _contaService = contaService;
            _repository = repository;
        }

        public async Task<Unit> Handle(TransferenciaCommand request, CancellationToken cancellationToken)
        {
            if (request.Valor <= 0)
                throw new Exception("Valor da transferência deve ser maior que zero.");

            if (request.ContaOrigemNumero == request.ContaDestinoNumero)
                throw new Exception("Conta de origem e destino não podem ser iguais.");

            var origemId = await _contaService.ObterIdPorNumeroAsync(request.ContaOrigemNumero);
            var destinoId = await _contaService.ObterIdPorNumeroAsync(request.ContaDestinoNumero);

            var existeTransferencia = await _repository.ExisteComIdempotentKeyAsync(origemId, request.IdempotentKey);
            if (existeTransferencia)
                throw new Exception("Transferência duplicada detectada (IdempotentKey já utilizado).");

            var transferencia = new Transferencia
            {
                ContaOrigemId = origemId,
                ContaDestinoId = destinoId,
                Valor = request.Valor,
                Data = DateTime.UtcNow,
                IdempotentKey = request.IdempotentKey
            };

            await _contaService.DebitarAsync(request.ContaOrigemNumero, request.Valor);
            await _contaService.CreditarAsync(request.ContaDestinoNumero, request.Valor);
            await _repository.RegistrarAsync(transferencia);

            return Unit.Value;
        }
    }
}

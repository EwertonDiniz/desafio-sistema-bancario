namespace TransferenciaAPI.Domain.Interfaces
{
    public interface IContaCorrenteService
    {
        Task<bool> ContaExisteAsync(Guid contaId);
        Task<bool> ContaEstaAtivaAsync(Guid contaId);
        Task<decimal> ObterSaldoAsync(Guid contaId);
        Task<string> DebitarAsync(string contaOrigemNumero, decimal valor);
        Task<string> CreditarAsync(string contaDestinoNumero, decimal valor);
        Task<string> ObterIdPorNumeroAsync(string numeroConta);
    }
}

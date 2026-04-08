using Dapper;
using System.Data;
using TransferenciaAPI.Domain.Entities;
using TransferenciaAPI.Domain.Interfaces;

namespace TransferenciaAPI.Infrastructure.Repositories
{
    public class TransferenciaRepository : ITransferenciaRepository
    {
        private readonly IDbConnection _connection;

        public TransferenciaRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task RegistrarAsync(Transferencia transferencia)
        {
            var sql = @"INSERT INTO transferencia (idtransferencia, idcontacorrente_origem, idcontacorrente_destino, datamovimento, valor, idempotentkey)
                    VALUES (@Id, @ContaOrigemId, @ContaDestinoId, @Data, @Valor, @IdempotentKey)";
            await _connection.ExecuteAsync(sql, transferencia);
        }

        public async Task<bool> ExisteComIdempotentKeyAsync(string contaOrigemId, string idempotentKey)
        {
            var sql = @"SELECT COUNT(1) FROM transferencia 
                    WHERE idcontacorrente_origem = @contaOrigemId AND idempotentkey = @idempotentKey";
            var count = await _connection.ExecuteScalarAsync<int>(sql, new { contaOrigemId, idempotentKey });
            return count > 0;
        }
    }
}

using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ContaCorrenteAPI.Infrastructure.Repositories;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly string _connectionString;
    public MovimentoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'Banco' não encontrada.");
    }
    public async Task RegistrarAsync(Movimento movimento)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, valor, tipomovimento, idempotentkey)
                    VALUES (@Id, @ContaId, @DataHora, @Valor, @Tipo, @IdempotentKey)";
        await conn.ExecuteAsync(sql, new
        {
            Id = movimento.Id.ToString(),
            ContaId = movimento.ContaId.ToString(),
            movimento.DataHora,
            movimento.Valor,
            movimento.Tipo,
            movimento.IdempotentKey
        });
    }

    public async Task<IEnumerable<Movimento>> ObterPorContaIdAsync(string contaId)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"SELECT idmovimento AS Id, idcontacorrente AS ContaId, datamovimento AS DataHora, valor AS Valor, tipomovimento AS Tipo, idempotentkey AS IdempotentKey
                     FROM movimento
                     WHERE idcontacorrente = @contaId";
        return await conn.QueryAsync<Movimento>(sql, new { contaId = contaId.ToString() });
    }

    public async Task<bool> ExisteMovimentoComIdempotentKeyAsync(string contaId, string key)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = "SELECT COUNT(1) FROM movimento WHERE idcontacorrente = @contaId AND idempotentkey = @key";
        var result = await conn.ExecuteScalarAsync<long>(sql, new { contaId = contaId.ToString(), key });
        return result > 0;
    }
}

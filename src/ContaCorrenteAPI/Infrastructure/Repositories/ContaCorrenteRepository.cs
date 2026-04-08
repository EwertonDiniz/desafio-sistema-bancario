using ContaCorrenteAPI.Domain.Entities;
using ContaCorrenteAPI.Domain.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ContaCorrenteAPI.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly string _connectionString;

    public ContaCorrenteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null.");
    }

    public async Task CriarAsync(ContaCorrente conta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO contacorrente (idcontacorrente, numero, nome, cpf, ativo, senha, salt)
                            VALUES (@Id, @Numero, @Nome, @Cpf, @Ativo, @SenhaHash, @Salt)";
        await conn.ExecuteAsync(sql, new
        {
            Id = conta.Id,
            Numero = int.Parse(conta.NumeroConta),
            Nome = conta.NomeTitular,
            conta.Cpf,
            conta.Ativo,
            conta.SenhaHash,
            conta.Salt
        });
    }

    public async Task AtualizarAsync(ContaCorrente conta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"UPDATE contacorrente SET nome = @Nome, ativo = @Ativo, senha = @SenhaHash, salt = @Salt
                            WHERE idcontacorrente = @Id";
        await conn.ExecuteAsync(sql, new
        {
            conta.Id,
            Nome = conta.NomeTitular,
            conta.Ativo,
            conta.SenhaHash,
            conta.Salt
        });
    }

    public async Task<IEnumerable<ContaCorrente>> ListarTodosAsync()
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"SELECT idcontacorrente AS Id, numero AS NumeroConta, nome AS NomeTitular, cpf AS Cpf, ativo AS Ativo, senha AS SenhaHash, salt AS Salt
                     FROM contacorrente";
        return await conn.QueryAsync<ContaCorrente>(sql);
    }

    public async Task<ContaCorrente> ObterPorIdAsync(string id)
    {
        using var conn = new SqliteConnection(_connectionString);

        var sql = @"SELECT idcontacorrente AS Id, numero AS NumeroConta, nome AS NomeTitular, cpf AS Cpf, ativo AS Ativo, senha AS SenhaHash, salt AS Salt
                    FROM contacorrente
                    WHERE idcontacorrente = @Id COLLATE NOCASE";

        var result = await conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Id = id });

        if (result == null)
        {
            throw new InvalidOperationException($"ContaCorrente with Id '{id}' was not found.");
        }

        return result;
    }
    
    public async Task<ContaCorrente?> ObterPorCpfAsync(string cpf)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"SELECT idcontacorrente AS Id, numero AS NumeroConta, nome AS NomeTitular, cpf AS Cpf, ativo AS Ativo, senha AS SenhaHash, salt AS Salt
                    FROM contacorrente
                    WHERE cpf = @Cpf";
        return await conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Cpf = cpf });
    }

    public Task<ContaCorrente?> ObterPorNumeroAsync(string numeroConta)
    {
        using var conn = new SqliteConnection(_connectionString);
        var sql = @"SELECT idcontacorrente AS Id, numero AS NumeroConta, nome AS NomeTitular, cpf AS Cpf, ativo AS Ativo, senha AS SenhaHash, salt AS Salt
                    FROM contacorrente
                    WHERE numero = @NumeroConta";
        return conn.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { NumeroConta = int.Parse(numeroConta) });
    }
}

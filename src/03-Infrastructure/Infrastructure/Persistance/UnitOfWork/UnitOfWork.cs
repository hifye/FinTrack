using System.Data;
using Application.Interfaces.UnitOfWork;

namespace Infrastructure.Persistance.UnitOfWork;

/// <summary>
/// Implementação do padrão Unit of Work para gerenciar transações de banco de dados.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;

    /// <summary>
    /// Obtém a transação atual do banco de dados.
    /// </summary>
    public IDbTransaction Transaction { get; private set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="UnitOfWork"/>.
    /// Garante que a conexão esteja aberta e inicia uma nova transação.
    /// </summary>
    /// <param name="connection">A conexão com o banco de dados.</param>
    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
        if(_connection.State == ConnectionState.Closed)
            _connection.Open();
        Transaction = _connection.BeginTransaction();
    }


    /// <summary>
    /// Confirma as alterações realizadas na transação atual de forma assíncrona.
    /// Em caso de erro, realiza o rollback automaticamente.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public Task CommitAsync()
    {
        try
        {
            Transaction.Commit();
            return Task.CompletedTask;
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            Transaction.Dispose();
            Transaction = _connection.BeginTransaction();
        }
    }

    /// <summary>
    /// Reverte as alterações realizadas na transação atual.
    /// </summary>
    public void Rollback()
    {
        Transaction?.Rollback();
        Transaction?.Dispose();
        Transaction = _connection.BeginTransaction();
    }

    /// <summary>
    /// Libera os recursos de transação e conexão.
    /// </summary>
    public void Dispose()
    {
        Transaction?.Dispose();
        _connection?.Dispose();
    }
}
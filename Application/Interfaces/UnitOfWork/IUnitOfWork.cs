using System.Data;

namespace Application.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IDbTransaction Transaction { get; }
    Task CommitAsync();
    void Rollback();
}
using System.Transactions;

namespace SubscriptionService.Domain.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    bool HasActiveTransaction { get; }
    public Task BeginTransaction(CancellationToken cancellationToken = default);

    public Task Commit(CancellationToken cancellationToken = default);
    public Task Rollback(CancellationToken cancellationToken = default);
}
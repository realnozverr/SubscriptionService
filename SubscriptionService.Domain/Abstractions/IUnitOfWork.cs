using System.Transactions;

namespace SubscriptionService.Domain.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    bool HasActiveTransaction { get; }
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    public Task CommitAsync(CancellationToken cancellationToken = default);
    public Task RollbackAsync(CancellationToken cancellationToken = default);
}
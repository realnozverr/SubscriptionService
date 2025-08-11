using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Infrastructure.Postgres.Outbox;

namespace SubscriptionService.Infrastructure.Postgres;

/// <summary>
/// Реализация паттерна Unit of Work для EF Core.
/// Обеспечивает атомарность операций и управляет транзакциями базы данных.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Проверяет, есть ли в данный момент активная, явно начатая транзакция.
    /// </summary>
    public bool HasActiveTransaction => _transaction != null;

    /// <summary>
    /// Начинает новую транзакцию базы данных.
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction active.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Сохраняет все изменения в базу данных и фиксирует текущую транзакцию.
    /// </summary>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction.");

        try
        {
            await SaveDomainEventsInOutboxAsync();
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    /// <summary>
    /// Откатывает текущую транзакцию.
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) return;

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    /// <summary>
    /// Метод для сбора и отправки ивентов в outbox
    /// </summary>
    private async Task SaveDomainEventsInOutboxAsync()
    {
        var outboxMessages = _context.ChangeTracker
            .Entries<IAggregate>()
            .Select(x => x.Entity)
            .SelectMany(aggregate =>
                {
                    var domainEvents = new List<DomainEvent>(aggregate.DomainEvents);;
                    aggregate.ClearDomainEvents();
                    return domainEvents;
                }
            )
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.EventId,
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();
        
        await _context.Set<OutboxMessage>().AddRangeAsync(outboxMessages);
    }

    private async ValueTask DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        // Если транзакция всё ещё открыта (забыли вызвать Commit/Rollback), откатываем её.
        if (_transaction != null)
        {
            await RollbackAsync();
        }

        await _context.DisposeAsync();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Освобождаем управляемые ресурсы, если вызван синхронный Dispose.
            _context.Dispose();
        }

        _disposed = true;
    }
}
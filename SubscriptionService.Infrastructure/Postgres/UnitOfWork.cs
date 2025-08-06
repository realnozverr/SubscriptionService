using Microsoft.EntityFrameworkCore.Storage;
using SubscriptionService.Domain.Abstractions;

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
    public async Task BeginTransaction(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction active.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Сохраняет все изменения в базу данных и фиксирует текущую транзакцию.
    /// </summary>
    public async Task Commit(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction.");

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await Rollback(cancellationToken);
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
    public async Task Rollback(CancellationToken cancellationToken = default)
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
            await Rollback();
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

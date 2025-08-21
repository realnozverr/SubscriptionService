using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.PlanAggregate;

namespace SubscriptionService.Infrastructure.Postgres.Repositories;

public class PlanRepository(DataContext context) : IPlanRepository
{
    public async Task<Plan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Plans
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Plan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Plans
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        await context.Plans.AddAsync(plan, cancellationToken);
    }
}
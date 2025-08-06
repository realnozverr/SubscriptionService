using SubscriptionService.Domain.Entities.PlanAggregate;

namespace SubscriptionService.Domain.Abstractions.Repositories;

public interface IPlanRepository : IRepository<Plan>
{
    Task<Plan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Plan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Plan plan, CancellationToken cancellationToken = default);
}
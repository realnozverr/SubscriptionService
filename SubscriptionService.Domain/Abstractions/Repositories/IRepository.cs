using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Abstractions.Repositories;

public interface IRepository<T> where T : IAggregate;

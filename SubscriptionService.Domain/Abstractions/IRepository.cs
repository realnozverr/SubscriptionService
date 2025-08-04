using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Abstractions;

public interface IRepository<T> where T : IAggregate;

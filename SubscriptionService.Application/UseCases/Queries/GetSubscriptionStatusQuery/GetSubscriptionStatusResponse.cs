using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace SubscriptionService.Application.UseCases.Queries.GetSubscriptionStatusQuery;

public record GetSubscriptionStatusResponse(
    bool HasActiveSubscription,
    int? PlanId,
    string PlanName,
    SubscriptionStatus Status,
    DateTime? StartDateUtc,
    DateTime? EndDateUtc);
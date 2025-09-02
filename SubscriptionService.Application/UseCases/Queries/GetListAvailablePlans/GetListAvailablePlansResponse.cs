using SubscriptionService.Domain.Entities.PlanAggregate;

namespace SubscriptionService.Application.UseCases.Queries.GetListAvailablePlans;

public record GetListAvailablePlansResponse(List<Plan> Plans);
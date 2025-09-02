using FluentResults;
using MediatR;
using SubscriptionService.Domain.Abstractions.Repositories;

namespace SubscriptionService.Application.UseCases.Queries.GetSubscriptionStatusQuery;

public class
    GetSubscriptionStatusHandler : IRequestHandler<GetSubscriptionStatusRequest, Result<GetSubscriptionStatusResponse>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPlanRepository _planRepository;

    public GetSubscriptionStatusHandler(
        ISubscriptionRepository subscriptionRepository,
        IPlanRepository planRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
    }

    public async Task<Result<GetSubscriptionStatusResponse>> Handle(GetSubscriptionStatusRequest request,
        CancellationToken cancellationToken)
    {
        var activeSubscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(request.UserId, cancellationToken);
        if (activeSubscription is null)
            return Result.Ok(new GetSubscriptionStatusResponse(
                HasActiveSubscription: false,
                PlanId: null,
                PlanName: null,
                Status: null,
                StartDateUtc: null,
                EndDateUtc: null));
        
        var plan = await _planRepository.GetByIdAsync(activeSubscription.PlanId, cancellationToken);
        if (plan is null)
            return Result.Fail($"Plan with ID '{activeSubscription.PlanId}' not found for an active subscription.");

        var response = new GetSubscriptionStatusResponse(
            HasActiveSubscription: true,
            PlanId: plan.Id,
            PlanName: plan.Name,
            Status:  activeSubscription.Status,
            StartDateUtc: activeSubscription.StartDate,
            EndDateUtc: activeSubscription.EndDate);
        
        return Result.Ok(response);
    }
}
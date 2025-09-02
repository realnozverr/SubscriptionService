using FluentResults;
using MediatR;
using SubscriptionService.Domain.Abstractions.Repositories;

namespace SubscriptionService.Application.UseCases.Queries.GetListAvailablePlans;

public class GetListAvailablePlansHandler : IRequestHandler<GetListAvailablePlansRequest, Result<GetListAvailablePlansResponse>>
{
    private readonly IPlanRepository _planRepository;
    public GetListAvailablePlansHandler(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }
    public async Task<Result<GetListAvailablePlansResponse>> Handle(GetListAvailablePlansRequest request, CancellationToken cancellationToken)
    {
        var plans = await _planRepository.GetAllAsync(cancellationToken);
        if (plans.Count() == 0)
            return Result.Fail("No plans found.");
        
        return Result.Ok(new GetListAvailablePlansResponse(plans.ToList()));
    }
}
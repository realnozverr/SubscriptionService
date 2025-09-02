using FluentResults;
using MediatR;

namespace SubscriptionService.Application.UseCases.Commands.CreateSubscriptionCommand;

public record CreateSubscriptionCommand(
    Guid UserId,
    int PlanId)
    : IRequest<Result<CreateSubscriptionResponse>>;
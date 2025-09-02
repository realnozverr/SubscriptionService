using FluentResults;
using MediatR;

namespace SubscriptionService.Application.UseCases.Queries.GetSubscriptionStatusQuery;

public record GetSubscriptionStatusRequest(Guid UserId) : IRequest<Result<GetSubscriptionStatusResponse>>;
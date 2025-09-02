using FluentResults;
using MediatR;

namespace SubscriptionService.Application.UseCases.Queries.GetListAvailablePlans;

public record GetListAvailablePlansRequest() : IRequest<Result<GetListAvailablePlansResponse>>;
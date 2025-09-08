using FluentResults;
using MediatR;

namespace SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;

public record GetOrCreateUserCommand(
    long TelegramId,
    string TelegramName
    ) : IRequest<Result<GetOrCreateUserResponse>>; 
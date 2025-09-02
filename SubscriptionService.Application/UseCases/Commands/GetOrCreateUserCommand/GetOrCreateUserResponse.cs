using SubscriptionService.Domain.Entities.UserAggregate;

namespace SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;

public record GetOrCreateUserResponse(Guid UserId, long TelegramId, UserStatus Status);
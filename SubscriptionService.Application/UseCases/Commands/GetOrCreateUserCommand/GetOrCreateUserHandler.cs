using FluentResults;
using MediatR;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;

public class GetOrCreateUserHandler : IRequestHandler<GetOrCreateUserCommand, Result<GetOrCreateUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public GetOrCreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<GetOrCreateUserResponse>> Handle(GetOrCreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTelegramIdAsync(request.TelegramId, cancellationToken);
        if (user is null)
        {
            user = User.Create(TelegramId.Create(request.TelegramId), null, UserStatus.Active);
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        return Result.Ok(new GetOrCreateUserResponse(user.Id, user.TelegramId.Value, user.Status.Name));
    }
}
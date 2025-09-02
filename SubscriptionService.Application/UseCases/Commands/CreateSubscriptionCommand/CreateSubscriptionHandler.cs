using FluentResults;
using MediatR;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace SubscriptionService.Application.UseCases.Commands.CreateSubscriptionCommand;

public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionCommand, Result<CreateSubscriptionResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    public CreateSubscriptionHandler(
        IUserRepository userRepository,
        IPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _userRepository = userRepository;
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }
    public async Task<Result<CreateSubscriptionResponse>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Fail($"User with id {request.UserId} not found");

        var plan = await _planRepository.GetByIdAsync(request.PlanId, cancellationToken);
        if (plan is null)
            return Result.Fail($"Plan with id {request.PlanId} not found");
        
        var activeSubscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(request.UserId, cancellationToken);
        if (activeSubscription is not null)
            return Result.Fail($"User already han an active subscription.");
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var subscription = Subscription.Create(user, plan, _timeProvider);
            await _subscriptionRepository.AddAsync(subscription, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return new CreateSubscriptionResponse(subscription.Id);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            return Result.Fail($"An error occurred while creating the subscription: {e.Message}");
        }
    }
}
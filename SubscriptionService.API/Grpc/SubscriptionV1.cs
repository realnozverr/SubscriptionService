using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Proto.SubscriptionV1;
using SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Infrastructure.Postgres;

namespace SubscriptionService.API.Grpc;

public class SubscriptionV1 : Proto.SubscriptionV1.SubscriptionService.SubscriptionServiceBase
{
    private readonly ILogger<SubscriptionV1> _logger;
    private readonly IMediator _mediator;
    public SubscriptionV1(ILogger<SubscriptionV1> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public override Task<CreatePaymentOrderResponse> CreatePaymentOrder(CreatePaymentOrderRequest request, ServerCallContext context)
    {
        return base.CreatePaymentOrder(request, context);
    }

    public override async Task<UserResponse> GetOrCreateUser(GetOrCreateUserRequest request, ServerCallContext context)
    {
        var response = await _mediator.Send(new GetOrCreateUserCommand(request.TelegramId));
        return response.IsSuccess
            ? new UserResponse
            {
                UserId = response.Value.UserId.ToString(),
                TelegramId = response.Value.TelegramId,
                Status = UserStatusProto.UserStatusActive,
                CreatedAt = DateTime.UtcNow.ToTimestamp()
            } : throw new RpcException(new Status(StatusCode.NotFound, "Не удалось найти или создать пользователя"));
    }

    public override Task<SubscriptionStatusResponse> GetSubscriptionStatus(GetSubscriptionStatusRequest request, ServerCallContext context)
    {
        return base.GetSubscriptionStatus(request, context);
    }

    public override Task<ListAvailablePlansResponse> ListAvailablePlans(ListAvailablePlansRequest request, ServerCallContext context)
    {
        return base.ListAvailablePlans(request, context);
    }
}
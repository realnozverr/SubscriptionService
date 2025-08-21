using Grpc.Core;
using MediatR;
using Proto.SubscriptionV1;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Infrastructure.Postgres;

namespace SubscriptionService.API.Grpc;

public class SubscriptionV1 : Proto.SubscriptionV1.SubscriptionService.SubscriptionServiceBase
{
    private readonly ILogger<SubscriptionV1> _logger;
    IMediator _mediator;
    public SubscriptionV1(ILogger<SubscriptionV1> logger)
    {
        _logger = logger;
    }
    public override Task<CreatePaymentOrderResponse> CreatePaymentOrder(CreatePaymentOrderRequest request, ServerCallContext context)
    {
        return base.CreatePaymentOrder(request, context);
    }

    public override Task<UserResponse> GetOrCreateUser(GetOrCreateUserRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.OK, "User already exists"));
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
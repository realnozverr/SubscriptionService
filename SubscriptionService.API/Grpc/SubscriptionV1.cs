using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Proto.SubscriptionV1;
using SubscriptionService.API.Mapper;
using SubscriptionService.Application.UseCases.Commands.CreateSubscriptionCommand;
using SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;
using SubscriptionService.Application.UseCases.Queries.GetListAvailablePlans;
using CreateSubscriptionResponse = Proto.SubscriptionV1.CreateSubscriptionResponse;

namespace SubscriptionService.API.Grpc;

public class SubscriptionV1 : Proto.SubscriptionV1.SubscriptionService.SubscriptionServiceBase
{
    private readonly ILogger<SubscriptionV1> _logger;
    private readonly IMediator _mediator;
    private readonly StatusMapper _statusMapper;

    public SubscriptionV1(ILogger<SubscriptionV1> logger, IMediator mediator, StatusMapper statusMapper)
    {
        _logger = logger;
        _mediator = mediator;
        _statusMapper = statusMapper;
    }

    public override Task<CreatePaymentOrderResponse> CreatePaymentOrder(CreatePaymentOrderRequest request,
        ServerCallContext context)
    {
        return base.CreatePaymentOrder(request, context);
        // TODO подумать об реализации оплаты в usecases уже сделал шаблон
    }

    public override async Task<UserResponse> GetOrCreateUser(GetOrCreateUserRequest request, ServerCallContext context)
    {
        var response = await _mediator.Send(new GetOrCreateUserCommand(request.TelegramId, request.TelegramUsername));
        return response.IsSuccess
            ? new UserResponse
            {
                UserId = response.Value.UserId.ToString(),
                TelegramId = response.Value.TelegramId,
                Status = _statusMapper.UserStatusMap(response.Value.Status),
                CreatedAt = DateTime.UtcNow.ToTimestamp()
            }
            : throw new RpcException(new Status(StatusCode.NotFound, "Не удалось найти или создать пользователя."));
    }

    public override async Task<SubscriptionStatusResponse> GetSubscriptionStatus(GetSubscriptionStatusRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User ID has an invalid format."));

        var response =
            await _mediator.Send(
                new Application.UseCases.Queries.GetSubscriptionStatusQuery.GetSubscriptionStatusRequest(userId));
        return response.IsSuccess
            ? new SubscriptionStatusResponse
            {
                PlanId = response.Value.PlanId.ToString(),
                PlanName = response.Value.PlanName,
                Status = _statusMapper.SubscriptionStatusMap(response.Value.Status),
                StartDateUtc = response.Value.StartDateUtc?.ToTimestamp(),
                EndDateUtc = response.Value.EndDateUtc?.ToTimestamp()
            }
            : throw new RpcException(new Status(StatusCode.NotFound, "Не удалось найти подписки."));
    }

    public override async Task<ListAvailablePlansResponse> ListAvailablePlans(ListAvailablePlansRequest request,
        ServerCallContext context)
    {
        var result = await _mediator.Send(new GetListAvailablePlansRequest());
        var response = new ListAvailablePlansResponse();
        if (result.IsSuccess)
        {
            response.Plans.AddRange(result.Value.Plans.Select(plan => new Plan
            {
                Id = plan.Id.ToString(),
                Name = plan.Name,
                Description = "",
                Price = (double)plan.Price,
                Currency = "RUB",
                DurationDays = plan.DurationInDays
            }));
            return response;
        }
        else throw new RpcException(new Status(StatusCode.NotFound, "Не удалось найти планы подписок."));
    }

    public override async Task<CreateSubscriptionResponse> CreateSubscription(CreateSubscriptionRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User ID has an invalid format."));

        var response = await _mediator.Send(new CreateSubscriptionCommand(userId, request.PlanId));
        return response.IsSuccess
            ? new CreateSubscriptionResponse
            {
                SubscriptionId = response.Value.SubscriptionId.ToString()
            }
            : throw new RpcException(new Status(StatusCode.Internal, "не удалось создать подписку."));
    }
}
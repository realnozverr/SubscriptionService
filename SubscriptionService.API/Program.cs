using MassTransit;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.API.Grpc;
using SubscriptionService.Application.UseCases.Commands.GetOrCreateUserCommand;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Infrastructure.Postgres;
using SubscriptionService.Infrastructure.Postgres.Repositories;
using Quartz;
using SubscriptionKafkaContracts.From.SubscriptionKafkaEvents;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Infrastructure.kafka;
using SubscriptionService.Infrastructure.Postgres.Outbox;

namespace SubscriptionService.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddGrpc();

        builder.Services.AddDbContextFactory<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        builder.Services.AddScoped<IPlanRepository, PlanRepository>();

        builder.Services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(OutboxBackgroundJob));
            configure
                .AddJob<OutboxBackgroundJob>(j => j.WithIdentity(jobKey))
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });
        
        builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        builder.Services.Configure<KafkaTopicsConfiguration>(builder.Configuration.GetSection("KafkaTopics"));
        builder.Services.AddTransient<IMessageBus, KafkaProducer>();
        builder.Services.AddMassTransit(x =>
        {
            x.UsingInMemory();
            x.AddRider(rider =>
            {
                rider.UsingKafka((context, k) =>
                {
                    k.Host(builder.Configuration.GetValue<string>("Kafka:BootstrapServers"));
                });
            });
        });
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(GetOrCreateUserHandler).Assembly
            )
        );

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
        app.MapGrpcService<SubscriptionV1>();
        app.Run();
    }
}
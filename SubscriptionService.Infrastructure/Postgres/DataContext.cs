using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Infrastructure.Postgres.EntityConfigurations;
using SubscriptionService.Infrastructure.Postgres.Outbox;

namespace SubscriptionService.Infrastructure.Postgres;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PlanConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxConfiguration());

        modelBuilder.Entity<Plan>().HasData(Plan.GetAll());
        modelBuilder.Ignore<DomainEvent>();
    }
}
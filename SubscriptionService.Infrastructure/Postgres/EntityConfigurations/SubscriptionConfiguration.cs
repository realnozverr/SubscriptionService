using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace SubscriptionService.Infrastructure.Postgres.EntityConfigurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(s => s.PlanId)
            .IsRequired()
            .HasColumnName("plan_id");

        builder.Property(s => s.Status)
            .HasConversion(
                v => v.Id,
                v => SubscriptionStatus.GetAll().Single(s => s.Id == v)
            )
            .IsRequired()
            .HasColumnName("status");

        builder.Property(s => s.StartDate)
            .IsRequired()
            .HasColumnName("start_date");

        builder.Property(s => s.EndDate)
            .IsRequired()
            .HasColumnName("end_date");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Plan>()
            .WithMany()
            .HasForeignKey(s => s.PlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
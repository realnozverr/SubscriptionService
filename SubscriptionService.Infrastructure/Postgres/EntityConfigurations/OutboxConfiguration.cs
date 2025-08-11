using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.Infrastructure.Postgres.Outbox;

namespace SubscriptionService.Infrastructure.Postgres.EntityConfigurations;

public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.HasIndex(x => x.OccurredOnUtc, "IX_outbox_messages_unprocessed")
            .IncludeProperties(x => new { x.Id, x.Type, x.Content })
            .HasFilter("processed_on_utc IS NULL");

        builder.Property(x => x.OccurredOnUtc)
            .HasColumnName("occurred_on_utc")
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc)
            .HasColumnName("processed_on_utc")
            .IsRequired(false);

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(x => x.Error)
            .HasColumnName("error")
            .IsRequired(false);
    }
}
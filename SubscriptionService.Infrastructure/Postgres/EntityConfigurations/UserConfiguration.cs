
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace SubscriptionService.Infrastructure.Postgres.EntityConfigurations;

public class 
    UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .HasColumnName("id");
        
        builder.Property(user => user.VpnIdentifier)
            .HasColumnName("vpn_identifier");
        
        builder.OwnsOne(user => user.TelegramId, telegramIdBuilder =>
        {
            telegramIdBuilder.Property(telegramId => telegramId.Value).HasColumnName("telegram_id").IsRequired();
        });
        
        builder.Property(user => user.Status)
            .HasConversion(
                v => v.Id,
                v => UserStatus.GetAll().Single(s => s.Id == v))
            .IsRequired()
            .HasColumnName("status");

        builder.Property(user => user.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
    }
}

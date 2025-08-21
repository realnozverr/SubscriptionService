using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionService.Domain.Entities.PlanAggregate;

namespace SubscriptionService.Infrastructure.Postgres.EntityConfigurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("plans");
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("name");

        builder.Property(p => p.Price).IsRequired()
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)");
        
        builder.Property(p => p.DurationInDays)
            .IsRequired()
            .HasColumnName("duration_in_days");
    }
}

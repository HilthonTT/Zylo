using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Events;
using Zylo.Domain.Notifications;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(n => n.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(n => n.Type)
            .HasConversion(p => p.Id, v => NotificationType.FromId(v)!)
            .IsRequired();

        builder.Property(n => n.Sent).HasDefaultValue(false);

        builder.Property(n => n.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}

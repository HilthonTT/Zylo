using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Events;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Configurations;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(a => a.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(a => a.Processed).IsRequired();

        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}

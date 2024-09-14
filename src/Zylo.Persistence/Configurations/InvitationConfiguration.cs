using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zylo.Domain.Events;
using Zylo.Domain.Invitations;
using Zylo.Domain.Users;

namespace Zylo.Persistence.Configurations;

internal sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(i => i.EventId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(i => i.Accepted).HasDefaultValue(false);

        builder.Property(i => i.Rejected).HasDefaultValue(false);

        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
